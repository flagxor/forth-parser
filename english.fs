#! /usr/bin/env gforth
require bnf.fs

bl token <BL>
bnf: _+   <BL> _+ | <BL> ;bnf
bnf: _*   <BL> _* | ;bnf
bnf: <NOUN>   t" cat" {{ s" cat " }}
            | t" dog" {{ s" dog " }}
            | t" pig" {{ s" pig " }}
            | t" chicken" {{ s" chicken " }} ;bnf
bnf: <VERB>   t" eats" {{ s" eat " }}
            | t" likes" {{ s" like " }}
            | t" hates" {{ s" hate " }} ;bnf
bnf: <SENTENCE>   <NOUN> _+ <VERB> _* t' .
                       {{ 2swap type ." you " type ." ?" }}
                | <NOUN> _+ <VERB> _* <NOUN> _* t' .
                       {{ >r >r 2swap type ." you " type r> r> type ." ?" }}
                ;bnf

: quoth: <parse <SENTENCE> parse> success @ 0= throw cr ;

quoth: cat eats.
quoth: dog eats dog.
quoth: pig hates dog.
cr bye
