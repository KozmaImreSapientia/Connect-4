# Connect-4
Solved with Mini-max Alpha-Beta Pruning<br><br>
<b>Connect Four</b> (also known as Captain's Mistress, Four Up, Plot Four, Find Four, Four in a Row, Four in a Line, Drop Four) is a two-player connection game in which the players first choose a color and then take turns dropping one colored disc from the top into a seven-column, six-row vertically suspended grid. The pieces fall straight down, occupying the lowest available space within the column. The objective of the game is to be the first to form a horizontal, vertical, or diagonal line of four of one's own discs.

## Heuristics
<pre>
The heuristic in minimax is used to evaluate the relative "goodness" of a board configuration.
    goodness = (four in a row)*1000000 + (three i a row)*10 + (two in a row)*5
    OR
    if my opponent has 1  four in a rows, then goodness = -1000000
</pre>
