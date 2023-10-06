INCLUDE globals.ink

{ name == "": -> main | -> already_chose }

-> main

===main===
Why do you bother with these people? You put in all this work and they give you nothing.

    * [Because people are worth it]
        -> bastard("") 
    * [Do I need a reason?]
        -> ishouldnt("die")
        

===bastard(fuckyoudie)===
Do you really believe that? {fuckyoudie}
 ~ name = fuckyoudie
 
    * [I have to.]
    -> dickballs("")
    * [I choose to.]
    -> markiplier("")
-> END

===dickballs(die)===
My condolences. {""}
 ~ name = die
-> END


===markiplier(markimoo)===
    You should really reconsider. {markimoo}
    
        * [I won't]
        -> Iwont("")
        * [Who hurt you?]
        -> youdid("")
   
    -> END
    
    ===Iwont(iwont)===
    Doomed optimist.

-> END

===ishouldnt(shouldnt)===
I'd at least hope you've thought of one. Though your brain doesn't work the best does it? Not like you to think something through is it?

    * [I do it because someone has to.]
        -> hasto("")
        
    * [I do it STRICTLY to annoy you]
        -> annoy("")
    * [Guess it's easy to have all the time in the world to think when all you do is sit in place.]
        -> mean("")

->END

===hasto(hastwo)===
No one on earth has to do all of this nonsense. Why waste our time? You could be doing anything.

    * [Like what]
     -> likewhat("")
    * [Yeah but I'm choosing to do this. Because it might make someones life better.]
     ->better("")


-> END

===better(bett)===
    Oh yeah, you're <j>GREAT</j> at that. Tell me if your method of crashing into peoples minds and scrambling their brains until it fits your memo works this time.
->END
===mean(realmean)===
    .... (The mirror doesn't reply. But you can tell by the sneering face they give you that what you said particularly got under their skin.) 

-> END

===likewhat(huh)===
..................................................................................................................................................... <j>OK I DON'T KNOW BUT ANYTHING IS BETTER THAN GOING OUT JUST TO GET HURT AGAIN ID RATHER STARE AT A CEILING ALL DAY THEN WITNESS ANOTHER FAMOUS FAILURE</j>

    * [no you wouldn't]
        -> andhowwouldyouknow("")
    
    
===andhowwouldyouknow(ijustwould)===
And how would <color=r><j>you</j></r> know?

    * [Because whether you'd like it or not, I'm me too. And I'm still going, whether or not you think you're afraid.]
    ->already_chose
-> END
===annoy(annoying)===
I guess I shouldn't find that surprising. You'd only be this good at it with such extreme practice. 


-> END
 ===youdid(Youdid)===
 You did. And I'm just waiting for you to do it again. Leaving your heart mindless open lets the dogs eat it with ease. But you never learned that did you? We're done talking. You never listen anyway.
 
 -> END
===already_chose===
...
-> END