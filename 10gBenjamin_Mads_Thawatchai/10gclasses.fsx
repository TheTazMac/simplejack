// 10g classes SimpleJack

module SimpleJackClasses
open System.Text
open System.Text.RegularExpressions
open System.Collections.Generic

// Adds a Random Number Generator
let rnd = System.Random()

//makes a list from 1..52
let cards = [|1..52|]
let cardses = new List<int>(0)
cardses.AddRange(cards)

/// <summary>
/// Creates a cache to save the progress of the drawn deck.
/// </summary>
/// <param name="i">Index in the array, assosiated to a card value</param>
/// <param name="casheCheck">Checks if the indexed value is in the cache if not add it to it.</param>
/// <returns>
/// Returns a cashe which changes when each card is drawn.
/// </returns>

let mutable cache = [0]
//cache to check whether the same cards gets generated or not
let rec cacheCheck (i: int) : int =
//if same card gets generated, then we go to next index
  if (List.contains i cache) = true then
    cacheCheck(i + 1)
  elif cardses.Count <= i then
  //catch-all, since we will never draw higher than 52
    0
  else
    cache <- i :: cache
    cardses.Item(i)

/// <summary>
/// Simulates the act of drawing a hand from a deck of cards.
/// </summary>
/// <param name="card1">simulates the first card</param>
/// <param name="card2">simulates the second card </param>
/// <returns>
/// The act of drawing a first and second card in a hand, which is 2 ints.
/// </returns>
type DrawHand = class
    val mutable card1 : int
    val mutable card2 : int

    new() =
        {card1 = cacheCheck(rnd.Next(1,52))
         card2 = cacheCheck(rnd.Next(1,52))
         }

    member this.FirstCard() = this.card1
    member this.SecondCard() = this.card2

end

/// <summary>
/// The act of drawing a random card from the deck.
/// </summary>
/// <param name="card1">A card drawn</param>
/// <returns>
/// The way to draw a card.
/// </returns>

type DrawCard = class
    val mutable card1 : int

    new() =
        {card1 = cacheCheck(rnd.Next(1,52))
         }

    member this.Card() = this.card1

end

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


