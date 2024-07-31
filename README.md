# UnispectDMAPlugin

Updated for MemProcFS v5.11

This is a plugin for Unispect, using the MemProcFS (PciLeech) Library.

**NOTE ON THIS FORK:** The original Unispect branch by Razchek has a bug where Memory Plugins are not disposed after dumping. This causes the FPGA connection to remain open until the program is closed again. This may cause problems with running another dump, OR using the FPGA in other programs. I have fixed this in my own fork of Unispect: https://github.com/imerzan/Unispect

Specifically in this commit: https://github.com/imerzan/Unispect/commit/0631aba2d73c39b87ad83f10f9463137f36cdcaa

## What can you do with it?
Dump Unity Mono Assemblies with Unispect from a second computer, using an FPGA DMA Card.

## Notes on this fork
- Place the following files into the Project Folder prior to building:
  - vmm.dll
  - leechcore.dll
  - FTD3XX.dll
  - Unispect.exe
  - symsrv.dll
  - dbghelp.dll
- You can download most of the above binaries at:
  - https://github.com/Razchek/Unispect/releases
  - https://github.com/ufrisk/MemProcFS/releases
  - https://ftdichip.com/drivers/d3xx-drivers/
- Build the solution for x64. Place all build files into the *Plugins* folder in your Unispect Directory.
- *(Optional)* If you have a Memory Map place it in the Unispect.exe directory (one level up from Plugins).
  - Memory Map should be named *mmap.txt*

### Have fun!
