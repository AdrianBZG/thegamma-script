﻿// ------------------------------------------------------------------------------------------------
// 
// ------------------------------------------------------------------------------------------------

namespace TheGamma
open Fable.Extensions

type Range = 
  { Start : int
    End : int }

type Error =
  { Number : int
    Message : string
    Range : Range }

type [<RequireQualifiedAccess>] TokenKind = 
  | LParen
  | RParen
  | Equals
  | Dot
  | Comma
  | Let
  | Operator of string
  | Boolean of bool
  | Number of string * float
  | Ident of string
  | QIdent of string
  | White of string
  | Newline

type Token = 
  { Token : TokenKind 
    Range : Range }

type Name = 
  { Name : string
    Range : Range }

type Argument<'T> = 
  { Name : Name option
    Value : Expr<'T> }

and Command<'T> = 
  { Command : CommandKind<'T>
    Range : Range }

and CommandKind<'T> = 
  | Let of Name * Expr<'T>
  | Expr of Expr<'T>

and [<RequireQualifiedAccess>] ExprKind<'T> = 
  | Variable of Name
  | Property of Expr<'T> * Name
  | Call of Expr<'T> * Name * Argument<'T> list
  | Number of float
  | Boolean of bool
  | Empty
  | Unit

and Expr<'T> =
  { Expr : ExprKind<'T>
    Range : Range 
    Type : 'T }

type Program<'T> = 
  { Body : Command<'T> list 
    Range : Range }


type Emitter = 
  { Emit : Babel.Expression * (string * Babel.Expression) list -> Babel.Expression }

type [<RequireQualifiedAccess>] Member = 
  | Property of name:string * typ:Type * emitter:Emitter
  | Method of name:string * arguments:(string * bool * Type) list * typ:Type * emitter:Emitter

and ObjectType = 
  { Members : Member list }

and [<RequireQualifiedAccess>] Type =
  | Delayed of Future<Type>
  | Primitive of string
  | Object of ObjectType
  | Function of arguments:Type list * returns:Type
  | Parameter of string
  | Unit
  | Any