Projeto SimProgramming - Equipa 6

**Curso : Engenharia Informática - UAb  
**Unidade Curricular: Lab. de Desenvolvimento de Software

**API Selecionada:** [PDFsharp](http://www.pdfsharp.net/)

## 📋 Sobre o Projeto
Este repositório contém a aplicação demonstradora desenvolvida em **C#** para o Tópico 2. O objetivo é criar um gerador de documentos PDF (Relatórios/Certificados) que ilustre a separação de responsabilidades através do padrão arquitetónico **MVC**.

## 👥 Constituição da Equipa
* **Líder:** Rui Fazendeiro
* **Desenvolvedores:** Andreia Correia, Frederico Antão, Telmo Silva
* **Verificador:** João Ferreira

## 🏗️ Estrutura do Projeto (MVC)
Para manter a organização, o código deve ser estruturado nas seguintes pastas dentro de `/src`:

* **`/Model`**: Classes de dados e lógica de negócio (ex: estrutura do relatório).
* **`/View`**: Interface de utilizador (Consola/UI) e renderização final do PDF via PDFsharp.
* **`/Controller`**: Orquestração entre o input do utilizador e a geração do documento.

## 🛠️ Tecnologias Utilizadas
* Linguagem: **C#** (.NET)
* Biblioteca: **PDFsharp** (Instalar via NuGet)
* IDE Recomendada: **Visual Studio 2022**

## 🚦 Regras de Fluxo (Git)
1. **Nunca** fazer commit direto na branch `main`.
2. Criar uma branch própria para cada tarefa: `feature/nome-da-tarefa`.
3. Abrir um **Pull Request** para revisão do Verificador antes do merge.
4. Garantir que o ficheiro `.gitignore` está ativo para evitar ficheiros temporários (`bin/obj`).

---
*Este projeto foi desenvolvido no âmbito da SimProgramming 2026.*
