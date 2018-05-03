module Tests.Main


open Fable.Core.JsInterop
open Fable.Markdown

type Test =
    { markdown : string
      html : string
      example: int
      start_line : int
      end_line : int
      section: string }

let tests : Test array = import "default" "./CommonMark_tests_0_28.json"

// QUnit.registerModule "Fable.Markdown tests"

// let singleTest (rank : int) =
//     QUnit.testCase ("Test n°" + string rank) <| fun asserter ->
//         let actual = Markdown.transformHtml tests.[rank].markdown
//         let expected = tests.[rank].html
//         asserter.equal actual expected

// singleTest 9

// // for test in tests do
// //     QUnit.testCase ("Test n°" + string test.example) <| fun asserter ->
// //         asserter.equal test.html test.html

// Markdown.transformHtml "#\tMaxime\n"
// |> printfn "%s"

Fable.Markdown.Lexer.tokenize
    """# Maxime1
## Maxime2"""
|> printfn "%A"


Fable.Markdown.Lexer.tokenize
    """# Maxime1
# foo *bar*
* Item 1
* Item 2"""
|> printfn "%A"


// [[2,"Maxime1",1,0]; [8,null,2,1]; [3,"Maxime2",2,1]; [1,null,-1,-1]]
