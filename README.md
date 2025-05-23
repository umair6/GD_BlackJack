# GAME LOOP

<pre lang="markdown"> ## 🃏 Blackjack Game Loop – Structured Flow ``` Game Start ├── Dealer uses DeckGenerator to create and shuffle the card deck. ├── GameManager deals initial cards to each player (e.g., 2 cards each). │ ├── Main Game Loop (per round) │ ├── Loop Start │ │ │ ├── For each player (including dealer, if acting separately) │ │ ├── GameManager gives turn to the player. │ │ ├── Wait for player input (Hit or Stand). │ │ ├── Process the input: │ │ │ ├── If Hit → deal card, update hand. │ │ │ ├── If Stand → end turn. │ │ ├── Check game state (e.g., bust, blackjack, max cards). │ │ ├── If game ends early → break loop to Result phase. │ │ │ ├── After all players finish or game ends │ │ ├── GameManager checks for final state (dealer logic, rules). │ │ └── Transition to result mode. │ ├── GameManager displays the result (Win/Lose/Tie). ├── Wait for user to start new round or exit. └── Loop End (next round or game over) ``` </pre>

# GAME COMPONENTS

## GameManager:

Manages game loop. It initially distirbute the cards to both players and then handles the palyer turns. Game continues until both players finis their move as per logic.

## HandVisual

Responsible for managing player hand visual state. It manages card animations, card stack, hand score text and hand state.

## PlayerController:

Manages the player hand UI and player hand state.

## AIPlayerController:

Inherited from PlayerController and user input related functiosn are overritten with AI logic.

## Dealer:

Manages the card dealing logic. It has a card deck generator which basically generates a suffled card deck. It has public functions to deal random card to players.

## CardsPool:

A card storage to reuse cards instead of creating new ones or throwing old ones away every time.

## WinEvaluator:

Class to evaluate winners. Current win evaluator is fairly simple but win evaluation code was seperated so it can be modified/updated easily when needed.

## HandEvaluators:

Simple evaluators for AI hit logic calculation. It uses IHandDataProvider for getting the player and ai hands data which is need to decided AI move.
This sytem can easily be explaned with more rules. Also with some modification we can create a simple comnfiguration file (JSON) for AI rules which can be modified from backend as well.

## CardSO:

A scriptable object to hold card data (sprite + value ). Created a EditorWindow script CardsDataGenerator which basically generte CardSO of all the cards.

# EXTERNAL CODE/LIBRARY

- DoTween: for card movement
- SafeArea: Script to adjust UI within safe area of device
