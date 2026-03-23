# Scriban_poc

A proof-of-concept application demonstrating the use of [Scriban](https://github.com/scriban/scriban), a fast and lightweight scripting language for .NET, for generating C# code from templates.

## Overview

This project uses Scriban templates to dynamically generate C# class files at runtime. It serves as a POC for template-based code generation.

## AI Assistance

Yes, this project is now using **Claude** (Anthropic's AI model) for AI-assisted development via GitHub Copilot, which supports multiple AI models including Claude. Claude helped implement and improve the code in this repository.

## Project Structure

- `ScribanPocApp/Program.cs` — Entry point that reads a Scriban template and generates a C# class file
- `ScribanPocApp/Templates/ClassTemplate.sbncs` — Scriban template for a C# class

## Getting Started

```bash
dotnet run --project ScribanPocApp
```

## Dependencies

- [Scriban](https://www.nuget.org/packages/Scriban) v6.4.0 — Scripting and template engine for .NET