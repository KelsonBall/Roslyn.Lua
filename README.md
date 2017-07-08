# Roslyn.Lua
## .Net Implementation of Lua 5.3ish

This project aims to bring lua firmly into the .net world by fully implementing it in C#. 

The project includes 
 * A parser that converts raw lua source code into an abstract syntax tree
 * An interpreter that executes parsed code at run time and interfaces nicely with other .net code
 * A compiler that translates parsed code into an IL executable using Roslyn
