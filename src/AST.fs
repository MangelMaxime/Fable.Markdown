module Fable.Markdown.AST

type Inline =
    | Text of body:string
    | CodeInline of body:string
    | Emphasis of body:Inline list
    | Strong of body:Inline list

type Block =
    | Heading of size:int * body:Inline list
    | Paragraph of body:Inline list
