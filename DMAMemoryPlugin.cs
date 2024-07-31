using System;
using System.IO;
using Unispect;
using Vmmsharp;

namespace unispectDMAPlugin
{
    [UnispectPlugin]
    public sealed class DMAMemoryPlugin : MemoryProxy
    {
        private const string MemMapPath = "mmap.txt";
        private readonly Vmm _vmm;
        private VmmProcess _proc;

        public DMAMemoryPlugin()
        {
            try
            {
                Log.Add("[DMA] Plugin Starting...");
                if (File.Exists(MemMapPath))
                {
                    Log.Add("[DMA] Memory Map Found!");
                    _vmm = new Vmm("-device", "FPGA", "-memmap", MemMapPath, "-waitinitialize");
                }
                else
                    _vmm = new Vmm("-device", "FPGA", "-waitinitialize");
                Log.Add("[DMA] Plugin Loaded!");
            }
            catch (Exception ex)
            {
                throw new DMAMemoryPluginException("[DMA] ERROR Initializing FPGA", ex);
            }
        }

        public override ModuleProxy GetModule(string moduleName)
        {
            try
            {
                Log.Add($"[DMA] Module Search: '{moduleName}'");
                var module = _proc.MapModuleFromName(moduleName);
                Log.Add($"[DMA] Module Found: '{module.sText}' | Base: 0x{module.vaBase.ToString("X")} | Size: {module.cbImageSize}");
                return new ModuleProxy(moduleName, module.vaBase, (int)module.cbImageSize);
            }
            catch (Exception ex)
            {
                throw new DMAMemoryPluginException($"[DMA] ERROR retrieving module '{moduleName}'", ex);
            }
        }

        public override bool AttachToProcess(string handle)
        {
            try
            {
                Log.Add($"[DMA] Attaching to process '{handle}'");
                // Slightly differs from Unispect's default Memory Plugin.
                // Use 'ProcessName.exe' instead of 'ProcessName'.
                if (_vmm.Process(handle) is not VmmProcess proc)
                    throw new Exception("Process not found!");
                _proc = proc;
                return true;

            }
            catch (Exception ex)
            {
                throw new DMAMemoryPluginException($"[DMA] ERROR attaching to process '{handle}'", ex);
            }
        }

        public override byte[] Read(ulong address, int length)
        {
            try
            {
                return _proc.MemReadCustom(address, length);
            }
            catch (Exception ex)
            {
                throw new DMAMemoryPluginException($"[DMA] ERROR Reading {length} bytes at 0x{address.ToString("X")}", ex);
            }
        }

        #region IDisposable
        private bool _disposed;
        public override void Dispose()
        {
            if (!_disposed)
            {
                Log.Add("[DMA] Dispose");
                _vmm.Dispose();
                _disposed = true;
            }
        }
        #endregion
    }
}