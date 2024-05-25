<!-- omit in toc -->
# ITA1 🔠

[![NuGet Version](https://img.shields.io/nuget/v/MichelMichels.Text.ITA1)](https://www.nuget.org/packages/MichelMichels.Text.ITA1)
[![.NET](https://github.com/MichelMichels/ITA1/actions/workflows/dotnet.yml/badge.svg)](https://github.com/MichelMichels/ITA1/actions/workflows/dotnet.yml)

This project contains a derived C# class of [`System.Text.Encoding`](https://learn.microsoft.com/en-us/dotnet/api/system.text.encoding) for the international version of [International Telegraph Alphabet no 1 (ITA1) (aka Baudot Code)](https://en.wikipedia.org/wiki/Baudot_code).

<details>

<summary>Table of Contents</summary>

- [Prerequisites](#prerequisites)
- [Building](#building)
- [Installation](#installation)
- [Usage](#usage)
- [Credits](#credits)


</details>

---

## Prerequisites
- [.NET 8.0](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)

## Building

Use [Visual Studio 2022](https://visualstudio.microsoft.com/vs/) to build the project. 

## Installation

Get the NuGet package from [nuget.org](https://www.nuget.org/) or search for `MichelMichels.Text.ITA1` in the GUI package manager in Visual Studio.

You can also use the cli of the package manager with following command:

```cli
Install-Package MichelMichels.Text.ITA1
```
<br />
<hr>

## Usage

> [!WARNING]
> This is an opinionated project and as such, I made the decision of not allowing lowercase letters. This decision 
> has the consequence that lowercase characters will not be converted to their equivalent uppercase character encoded 
> byte, they will just be left out. This leads to loss of data when encoding and decoding.


Below is a short example on how to use this `Ita1Encoding` class.

```csharp
using MichelMichels.Text;

Encoding ita1 = new Ita1Encoding();

// Encoding characters
byte[] bytes = ita1.GetBytes("HELLO WORLD");

// Decoding bytes
string value = ita1.GetString([0x0B, 0x02, 0x1B, 0x1B, 0x07, 0x10, 0x16, 0x07, 0x1C, 0x1B, 0x0F]);
```

Following example writes and reads text encoded in ITA1 to and from a text file.
```csharp
using MichelMichels.Text;

Encoding ita1 = new Ita1Encoding();

// Writing
File.WriteAllText("output.txt", "HELLO WORLD", ita1);

// Reading
string data = File.ReadAllText("output.txt", ita1);
```

## Credits

- Written by [Michel Michels](https://github.com/MichelMichels).