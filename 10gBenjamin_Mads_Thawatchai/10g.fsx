// fsharpc 10gclasses.fsx 10g.fsx && mono 10g.exe
// 10g

open System
open System.Text
open System.Text.RegularExpressions
open System.Collections.Generic
open SimpleJackClasses

// -------------------OPTIONS---------------------
// Add regular expressions to allow mistypes when reading user input
let numberReg = Regex "^[1-5]$"
let startReg = Regex "((?i)(\w+)?s(\w+)?)"
let standReg = Regex "((?i)(\w+)?s(\w+)?)"
let hitReg = Regex "((?i)(\w+)?h(\w+)?)"
let aiReg = Regex "((?i)(\w+)?a(\w+)?)"
let unitReg = Regex "((?i)unit)"
let aReg = Regex "((?i)a)"
let bReg = Regex "((?i)b)"

// Sleep values for low/med/high making it easily changeable
let lowSleep = 500
let medSleep = 1000
let hiSleep = 1500

// Add the 4 different Card suits in different lists, to make it easier if we want to change something later.
let clubs = ["Ace of Clubs"; "2 of Clubs"; "3 of Clubs"; "4 of Clubs"; "5 of Clubs"; "6 of Clubs"; "7 of Clubs"; "8 of Clubs"; "9 of Clubs"; "10 of Clubs"; "Jack of Clubs"; "Queen of Clubs"; "King of Clubs"]

let diamonds = ["Ace of Diamonds"; "2 of Diamonds"; "3 of Diamonds"; "4 of Diamonds"; "5 of Diamonds"; "6 of Diamonds"; "7 of Diamonds"; "8 of Diamonds"; "9 of Diamonds"; "10 of Diamonds"; "Jack of Diamonds"; "Queen of Diamonds"; "King of Diamonds"]

let hearts = ["Ace of Hearts"; "2 of Hearts"; "3 of Hearts"; "4 of Hearts"; "5 of Hearts"; "6 of Hearts"; "7 of Hearts"; "8 of Hearts"; "9 of Hearts"; "10 of Hearts"; "Jack of Hearts"; "Queen of Hearts"; "King of Hearts"]

let spades = ["Ace of Spades"; "2 of Spades"; "3 of Spades"; "4 of Spades"; "5 of Spades"; "6 of Spades"; "7 of Spades"; "8 of Spades"; "9 of Spades"; "10 of Spades"; "Jack of Spades"; "Queen of Spades"; "King of Spades"]

// Card Deck that holds the card-strings in order of a sorted deck.
let cardDeck = new List<string>()
cardDeck.Add("0") // account for 0 index
cardDeck.AddRange(clubs)
cardDeck.AddRange(diamonds)
cardDeck.AddRange(hearts)
cardDeck.AddRange(spades)

//sets the amount of players to 0 at the start of the game
let mutable amountofPlayers = 0
// makes a var player
let mutable player = 1
// list to hold and save results
let result = Results()
result.Save(0) // account for 0 index

// Actual random shuffled deck of ints
let DeckofCards = CardDeck()
do DeckofCards.Shuffle()


// ---------------------------OPTIONS-----------------------

//introduction text
printfn ""
do Console.WriteLine "Welcome to SimpleJack!"
printfn ""
do Console.WriteLine "Write 'start' to begin or 'unit' to run tests..."
//user input to begin the game
let mutable r = Console.ReadLine()

//-------------------------------- How many players ------------------------------------------------
while 1 > amountofPlayers do
  match r with
  //startReg sets parameter that "start" has to be in the user input
  | _ when (startReg.IsMatch r = true) -> Console.WriteLine "How many players?"
                                          //user input
                                          let r2 = Console.ReadLine()
                                          match r2 with
                                          //numberReg sets limit to 5 players
                                          | _ when (numberReg.IsMatch r2 = true) -> amountofPlayers <- (int(r2))
                                          //error message if a number 1..5 is not written
                                          | _ -> Console.WriteLine "That amount is not supported."
  | _ when (unitReg.IsMatch r = true) -> Console.WriteLine "\n Testing our Card deck..\n"
                                         wait(1500)
                                         // test first two cards
                                         printfn "First card: %b \n" ((cardDeck.Item(1)) = "Ace of Clubs")
                                         printfn "Last card: %A \n" ((cardDeck.Item(52)) = "King of Spades")
                                         let mutable drawtest = 0
                                         Console.WriteLine "Drawing 3 cards... \n"
                                         // draw 3 cards and see if they're real value and not in the deck after
                                         while drawtest < 3 do
                                             let e = DeckofCards.Draw()
                                             printfn "We drew: %A" (cardDeck.Item(e))
                                             wait(500)
                                             printfn "RealValueConverter: %A" (RealValueConverter(cardDeck.Item(e)))
                                             wait(500)
                                             printfn "It is not in the deck anymore: %b" (not (List.contains e (DeckofCards.getDeck())))
                                             wait(1000)
                                             printfn ""
                                             drawtest <- drawtest + 1
                                         // drawing all 4 aces in a row, check if we bust
                                         let unitfirst = DeckofCards.Draw(1)
                                         let unitsecond = DeckofCards.Draw(14)
                                         let mutable unitVal = (RealValueConverter(cardDeck.Item(unitfirst))) + (RealValueConverter(cardDeck.Item(unitsecond)))
                                         // makes sure we don't bust if we draw an ace that puts us above 21
                                         if ((RealValueConverter(cardDeck.Item(unitfirst))) + (RealValueConverter(cardDeck.Item(unitsecond)))) = 22 then
                                           unitVal <- unitVal - 10
                                         printfn "Draw an Ace: %b" ((cardDeck.Item(unitfirst)) = "Ace of Clubs")
                                         printfn "Our hand has the value 11: %b" ((RealValueConverter(cardDeck.Item(unitfirst))) = 11)
                                         printfn "Draw another ace: %b" ((cardDeck.Item(unitsecond)) = "Ace of Diamonds")
                                         printfn "Our hand now has the value 12: %b \n" (unitVal = 12)
                                         let mutable unitDraw = DeckofCards.Draw(27)
                                         unitVal <- unitVal + RealValueConverter(cardDeck.Item(unitDraw))
                                         if (RealValueConverter(cardDeck.Item(unitDraw)) = 11) && unitVal > 21 then
                                           unitVal <- unitVal - 10
                                         wait(500)
                                         printfn "Draw another ace: %b" ((cardDeck.Item(unitDraw)) = "Ace of Hearts")
                                         printfn "Our hand now has the value 13: %b" (unitVal = 13)
                                         unitDraw <- DeckofCards.Draw(40)
                                         unitVal <- unitVal + RealValueConverter(cardDeck.Item(unitDraw))
                                         if (RealValueConverter(cardDeck.Item(unitDraw)) = 11) && unitVal > 21 then
                                           unitVal <- unitVal - 10
                                         wait(500)
                                         printfn "Draw another ace: %b" ((cardDeck.Item(unitDraw)) = "Ace of Spades")
                                         printfn "Our hand now has the value 14: %b \n" (unitVal = 14)
                                         Console.WriteLine "UNIT TEST COMPLETE: \n Write 'start' to play or 'quit' to stop."
                                         let r5 = Console.ReadLine()
                                         r <- r5
  //program shuts down if startReg is not fulfilled
  | _ -> failwith "Quitting..."

//text to set whether it is player or players
if amountofPlayers = 1 then
  printfn "Starting game with %i player.." amountofPlayers
else
  printfn "Starting game with %i players.." amountofPlayers
printfn ""

/// <summary>
/// The game initiates until all players have ended their turn. Drawn their hand. Used either "ai",
/// "stand" or "hit", choosen by the player themselves. And in the end asigned a player value to each player.
/// </summary>
/// <remarks>
/// Uses a lot of pattern matching. Firstly its to get from either "ai" to a player playing. And then if it busts or stands.
/// A lot of lines is also used to compute the Ace, which can either be 11 or 1.
/// It's also important to note the difference in ai strategies "a" or "b".
/// This all adds up, so that we have a ton of code for each match, cause we have to take mostly all of these things into account for each match.
/// </remarks>
/// <param name="player">the current players number</param>
/// <param name="playerVal">The value of the cards given from the hand</param>
/// <param name="Hand">Draws a hand, from thhe poll of cards </param>
/// <param name="a and b">a is the first drawn card, b is the second drawn </param>
/// <returns>
/// Returns each playerValue to each player count. A result for each item(i).
/// </returns>

//---------------------------------------- Player drawing cards -------------------------------------------------
while player <= amountofPlayers do
  printfn ""
  printfn "------------------------------"
  printfn "Player %i" player
  //drawing our cards by calling the DrawHand class
  let firstcard = DeckofCards.Draw()
  let secondcard = DeckofCards.Draw()
  //Calling the RealValueConverter class to the playerVal function in regards to FirstCard and SecondCard
  let mutable playerVal = (RealValueConverter(cardDeck.Item(firstcard))) + (RealValueConverter(cardDeck.Item(secondcard)))
  //outputs what cards the player gets. Added sleep to give it a better feeling
  printfn ""
  wait(medSleep)
  printfn "Player %i drew: %A" player (cardDeck.Item(firstcard))
  wait(medSleep)
  printfn "Player %i drew: %A" player (cardDeck.Item(secondcard))
  wait(medSleep)
  printfn "Player %i Your card value is: %i" player playerVal
  //a divider to give a better overview after each step
  printfn "------------------------------"
  printfn ""
  // if you draw 2 aces, 1 of them is value 1
  if ((RealValueConverter(cardDeck.Item(firstcard))) + (RealValueConverter(cardDeck.Item(secondcard)))) = 22 then
    playerVal <- playerVal - 10
  //if the total value of the players card is <= 21, then the player gets 3 options
  //---------------------------------------- Hit, Stand or AI ----------------------------------------------------------
  while playerVal <= 21 do
    Console.WriteLine "You can 'Hit' or 'Stand' or have an 'AI' play for you."
    let r3 = Console.ReadLine()
    match r3 with
    //Hit option: matching with regular expression so misstypes also will be acknowledged. Calls the DrawCard class
    | _ when (hitReg.IsMatch r3 = true) -> let drawncard = DeckofCards.Draw()
                                           //output what player drew when choosing Hit option
                                           printfn ""
                                           //calling cardDeck to output what card the player gets
                                           printfn "Player %i drew: %A" player (cardDeck.Item(drawncard))
                                           //takes playerVal and calls the RealValueConverter to add it with the cardDeck item
                                           playerVal <- playerVal + (RealValueConverter(cardDeck.Item(drawncard)))
                                           //If you have 2 aces then one of them is value 1
                                           if (RealValueConverter(cardDeck.Item(drawncard)) = 11) && playerVal > 21 then
                                             playerVal <- playerVal - 10
                                             printfn "Your card value is: %i" playerVal
                                             printfn "------------------------------"
                                           //if playerVal is > 21 then player went bust
                                           elif playerVal > 21 then
                                             result.Save(playerVal)
                                             printfn "Player %i you went bust with %i!" player playerVal
                                             printfn "------------------------------"
                                             wait(hiSleep)
                                           else
                                             //outputs players card value
                                             wait(hiSleep)
                                             printfn "Your card value is: %i" playerVal
                                             printfn "------------------------------"
    //stand option: matching with regular expression so misstypes also will be acknowledged
    | _ when (standReg.IsMatch r3 = true) -> printfn ""
                                            //shows the players card value when player stands
                                             result.Save(playerVal)
                                             printfn "Player %i stood with %A." player playerVal
                                             printfn "------------------------------"
                                             printfn ""
                                             playerVal <- 51
    //ai option: matching with regular expression so misstypes also will be acknowledged; player can choose strategy A or B
    | _ when (aiReg.IsMatch r3 = true) -> Console.WriteLine "Strategy 'A' or 'B'?"
                                          let r4 = Console.ReadLine()
                                          match r4 with
                                          //matches with aReg, a regular expression, so either a or A can be typed
                                          | _ when (aReg.IsMatch r4 = true) ->
                                            while playerVal <= 50 do
                                                      //Ai option A. This parameter makes the ai stop, when 17 <= playerVal && 21 >= playerVal
                                                     if 17 <= playerVal && 21 >= playerVal then
                                                       printfn ""
                                                       printfn "------------------------------"
                                                       printfn "Player %i stood with %i" player playerVal
                                                       result.Save(playerVal)
                                                       printfn "------------------------------"
                                                       wait(hiSleep)
                                                       playerVal <- 51
                                                       //if playerVal is > 21 then AI went bust
                                                     else if playerVal > 21 then
                                                       printfn ""
                                                       printfn "------------------------------"
                                                       printfn "Player %i went bust with %i" player playerVal
                                                       result.Save(playerVal)
                                                       printfn "------------------------------"
                                                       wait(hiSleep)
                                                       playerVal <- 51
                                                     else
                                                     //DrawCard class gets called to give the AI cards
                                                       let drawncard = DeckofCards.Draw()
                                                       wait(hiSleep)
                                                       printfn "------------------------------"
                                                       printfn "Player %i drew: %A" player (cardDeck.Item(drawncard))
                                                       playerVal <- playerVal + (RealValueConverter(cardDeck.Item(drawncard)))
                                                       printfn "Player %i card value is: %i" player playerVal
                                                       printfn "------------------------------"
                                                       wait(medSleep)
                                          //bReg gets matched, a regular expression, so either b or B gets acknowledged. AI option B
                                          | _ when (bReg.IsMatch r4 = true) ->
                                            while playerVal <= 50 do
                                                     //DrawCard class gets called to give AI cards
                                                     let drawncard = DeckofCards.Draw()
                                                     //if playerVal is > 21 then AI goes bust
                                                     if playerVal > 21 then
                                                       printfn ""
                                                       printfn "------------------------------"
                                                       printfn "Player %i went bust with %i" player playerVal
                                                       result.Save(playerVal)
                                                       printfn "------------------------------"
                                                       wait(hiSleep)
                                                       playerVal <- 51
                                                       //AI picks between 0 and 2 where 0 = hit and 2 = stand
                                                     else if (rnd.Next(0,2)) = 0 then
                                                       printfn "------------------------------"
                                                       printfn "Player %i drew %A" player (cardDeck.Item(drawncard))
                                                       playerVal <- playerVal + (RealValueConverter(cardDeck.Item(drawncard)))
                                                       printfn "Player %i card value is: %i" player playerVal
                                                       printfn "------------------------------"
                                                       wait(medSleep)
                                                     else
                                                     //outputs the value that AI stands with
                                                       printfn ""
                                                       printfn "------------------------------"
                                                       printfn "Player %i stood with %i" player playerVal
                                                       result.Save(playerVal)
                                                       printfn "------------------------------"
                                                       wait(medSleep)
                                                       playerVal <- 51
                                          | _ -> Console.WriteLine "Unknown Strategy."
    //message error if player writes something that isn't allowed
    | _ -> Console.WriteLine "Unknown Command."
  //changes player value after the round
  player <- player + 1

//------------------------------ Player Results --------------------------------------------------------
printfn ""
printfn "RESULTS:"
//loop that outputs the result of the players
result.ReverseList()

for i=1 to amountofPlayers do
  wait(lowSleep)
  printfn "Player %i ended on %A" i (result.Item(i))


/// <summary>
/// Gives the dealer a hand, his value and makes his descisions based on whether its below or above 17. He always stands at 17 or above.
/// </summary>
/// <remarks>
/// Takes the same 3 senarioes as the player algoritm. If above 21, If hit or if he stands. This just checks if he has to stand or hit, instead of choosing yourself.
/// </remarks>
/// <param name="dealerHand">The hand given to the dealer</param>
/// <param name="dealerVal">The value on the dealers cards</param>
/// <param name="dealerResult">The end result of the dealerVal</param>
/// <returns>
/// returns the dealerResult.
/// </returns>

//--------------------------------------------- Dealer ---------------------------------------------------------------------
printfn ""
printfn "Dealer's turn.."
printfn ""
//calls the DrawHand Class with the FirstCard and SecondCard
let dealerfirst = DeckofCards.Draw()
let dealersecond = DeckofCards.Draw()
//RealValueConverter gets called and converts the cards that the Dealer gets
let mutable dealerVal = (RealValueConverter(cardDeck.Item(dealerfirst))) + (RealValueConverter(cardDeck.Item(dealersecond)))
result.setDealer(dealerVal)
printfn ""
wait(hiSleep)
//Dealer gets their cards with the value put together
printfn "Dealer drew: %A" (cardDeck.Item(dealerfirst))
wait(hiSleep)
printfn "Dealer drew: %A" (cardDeck.Item(dealersecond))
wait(medSleep)
printfn "Dealer's card value is: %i" dealerVal
printfn ""
while dealerVal <= 50 do
  //if dealerVal is >= 17 and <= 21 then dealer should stand
  if 17 <= dealerVal && 21 >= dealerVal then
    wait(hiSleep)
    printfn ""
    printfn "Dealer stood with: %i" dealerVal
    dealerVal <- 51
    //if dealerVal is > 21 then dealer goes bust
  else if dealerVal > 21 then
    wait(hiSleep)
    printfn "Dealer went bust with %i!" dealerVal
    dealerVal <- 51
  else
  //drawing cards for the dealer
    let dealerdraw = DeckofCards.Draw()
    wait(hiSleep)
    printfn "Dealer drew: %A" (cardDeck.Item(dealerdraw))
    dealerVal <- dealerVal + (RealValueConverter(cardDeck.Item(dealerdraw)))
    // making sure dealer doesn't if ace puts dealer above 21.
    if (RealValueConverter(cardDeck.Item(dealerdraw)) = 11) && dealerVal > 21 then
      dealerVal <- dealerVal - 10
    //update dealer
    result.setDealer(dealerVal)
    wait(medSleep)
    //dealer value output
    printfn "Dealer card value is: %i" dealerVal

/// <summary>
/// Result list outputs whether the players won against dealer or lost against him or if the dealer/player went bust, via the class
/// member Print.
/// </summary>
/// <returns>
/// returns a list of each players standings against the dealer's result and their own result.
/// </returns>

//---------------------------------------------- Result Screen ---------------------------------------------------------------
printfn ""
printfn ""
printfn "______________RESULTS_______________ \n"
//Prints the results of all the players in the game
result.Print(amountofPlayers)
