# Scriban_poc

A proof-of-concept application that combines **Claude AI** (Anthropic) with **Scriban** templates to generate C# class code from natural language descriptions.

## How it works

1. You provide a natural language description of the C# class you want (e.g. *"A Car class with a string property called Model"*)
2. The app sends your description to **Claude** (via the Anthropic API), which extracts the structured class specification
3. The extracted specification is fed into a **Scriban** template to produce a ready-to-use C# class file

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- An [Anthropic API key](https://console.anthropic.com/)

## Setup

Set your Anthropic API key as an environment variable:

```bash
export ANTHROPIC_API_KEY=your_api_key_here
```

## Usage

Run with the default description (*"A Person class with a string property called Name"*):

```bash
dotnet run --project ScribanPocApp
```

Or provide your own description:

```bash
dotnet run --project ScribanPocApp -- "A Car class with a string property called Model"
```

The generated C# class file is written to `GeneratedClasses/GeneratedClass.cs` inside the output directory.

## Dependencies

| Package | Version | Purpose |
|---------|---------|---------|
| [Anthropic.SDK](https://github.com/tghamm/Anthropic.SDK) | 5.10.0 | Claude AI integration |
| [Scriban](https://github.com/scriban/scriban) | 6.4.0 | Template engine |
