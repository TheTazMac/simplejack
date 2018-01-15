// 10g classes SimpleJack

module SimpleJackClasses
open System.Text
open System.Text.RegularExpressions
open System.Collections.Generic

// Adds a Random Number Generator
let rnd = System.Random()

type CardDeck = class
    val mutable deck : int list
    val random : System.Random

    new() =
        {
         random = System.Random();
         deck = []
        }

    member this.Shuffle() =
        this.deck <- List.sortBy (fun x -> this.random.Next()) [1..52]

    member this.Draw() =
        if this.deck.IsEmpty then
            this.Shuffle()
        let draw = this.deck.Head
        this.deck <- this.deck.Tail
        draw

end
(*
type UnitTest = class
    val mutable card1 : int
    val mutable card2 : int
    val mutable deck : int list

    new((card1)(card2)) =
        {
         card1 = firstcard
         card2 = secondcard
        }

    member this.FirstCard() = this.card1
    member this.SecondCard() = this.card2

    member this.Draw() =
        if this.deck.IsEmpty then
            this.Shuffle()
        let draw = this.deck.Head
        this.deck <- this.deck.Tail
        draw

end
*)
let extract (r:Regex) (s:string) : string option =
  let m = r.Match s
  in if m.Success then Some (string(m.Groups.[1]))
    else None

/// <summary>
/// regular expression function that gives value to the strings
/// </summary>
/// <param name="numbReg">The normal number cards distinguished from 1-10 by a regex, by points of given number</param>
/// <param name="royalReg">regex by J or K or Q, to royals by points 10</param>
/// <param name="aceReg">regex the ace to 11</param>
/// <param name=""></param>
/// <returns>
/// The way to distinguish a string, to a given card.
/// </returns>

let RealValueConverter (card: string) : int =
  let numbReg = Regex "^([1-9]([0]?))"
  let royalReg = Regex "^([J|K|Q])"
  let aceReg = Regex "^(Ace)*"
  let a = match extract numbReg card with
          | Some a -> a
          | None -> card
  match a with
  | _ when (numbReg.IsMatch a = true) -> int(a)
  | _ when (royalReg.IsMatch a = true) -> 10
  | _ when (aceReg.IsMatch a = true) -> 11
  | _ -> 0
