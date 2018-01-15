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

    // Creates a new deck to be shuffled
    new() =
        {
         random = System.Random();
         deck = []
        }

    // insert cards and shuffles them randomly
    member this.Shuffle() =
        this.deck <- List.sortBy (fun x -> this.random.Next()) [1..52]

    // simulates the act of drawing the top card and then removes it
    member this.Draw() =
        if this.deck.IsEmpty then
            this.Shuffle()
        let draw = this.deck.Head
        this.deck <- this.deck.Tail
        draw

    member this.getDeck() = this.deck

    // for testing if we want to draw a specific card/number
    member this.Draw(a: int) = a

end

type Results = class
    val mutable list : int list
    val mutable dealerResult : int

    new() =
        {
         list = []
         dealerResult = 0
        }

    member this.setDealer(a: int) = this.dealerResult <- a

    member this.getDealer() = this.dealerResult

    // to save results for end
    member this.Save(a: int) = this.list <- a :: this.list

    // since :: inserts as head, we need to rev so we can match player with result
    member this.ReverseList() = this.list <- List.rev this.list

    member this.Item(a: int) = this.list.[a]

    // for printing the final results and their status
    member this.Print(a: int) =
        for i=1 to a do
            if this.Item(i) > 21 then
                printfn "Player %i went bust." i
            elif this.Item(i) > this.getDealer() then
                printfn "Player %i won with %A vs dealers %A." i (this.Item(i)) (this.getDealer())
            elif this.getDealer() > 21 && this.Item(i) <= 21 then
                printfn "Dealer went bust and Player %i won!" i
            else
                printfn "Player %i lost with %A to dealer's %A." i (this.Item(i)) (this.getDealer())

end

// shortcut for sleeping
let wait (a: int) =
    System.Threading.Thread.Sleep(a)

// helper-function that extract matches as strings
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
