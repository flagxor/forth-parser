#! /usr/bin/env gforth
require bnf.fs

bl token <BL>
bnf: _+   <BL> _+ | <BL> ;bnf
bnf: _*   <BL> _* | ;bnf
bnf: <NOUN>   t" cat" | t" dog" | t" pig" | t" chicken" ;bnf
bnf: <VERB>   t" eats" | t" likes" | t" hates" ;bnf
bnf: <SENTENCE>   <NOUN> _+ <VERB> _* t' .
                | <NOUN> _+ <VERB> _* <NOUN> _* t' .
                ;bnf

: quoth: <parse <SENTENCE> parse> ;

quoth: cat eats.
quoth: dog eats dog.
quoth: pig hates dog.
