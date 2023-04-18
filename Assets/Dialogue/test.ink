INCLUDE globals.ink

{ name == "": -> main | -> already_chose }

-> main

=== main ===
Which Zachary do you choose?
    + [Firewolf]
        -> chosen("a god")
    + [Raccoon]
        -> chosen("a trash animal")
    + [Cowardwolf]
        -> chosen("a garbage Fizz player")
        
=== chosen(zach) ===
He is {zach}!
~ name = zach
-> END

=== already_chose ===
Zachary is {name}!
-> END