INCLUDE Army
INCLUDE RE_Refugees

EXTERNAL place_Char(char_name, location)
//combat, normal dialogue 
EXTERNAL set_dialogue_type(type)

#characters
VAR player = "Eric"
VAR daughter = "Diana"
VAR woodElf = "Kerillian"
VAR chief = "CHIEF"

#locations
VAR we_forest = "Athel Loren"
VAR city = "Ubersriek"
-> village
=== tavern ===
//characters:
//place_Char(chief, 0)
This tavern has seen better days, but its still among the better places in the city. You all crowd around a table lamenting your fortunes. 
*   [Complain about the docks]
    Fishermen come back with frightening stories of strange creatures out there. Hopefully nothing more than stories.
*   (guard)[Grumble about guard duty]
    High taxes and poor harvests make for a hard year. The desperate are driven more frequently to crime.
*   [Moan about your ill fortune]
    Trade has been slow and jobs are few.
    
#You friend, NAME, works on in the guard
//{place_Char("Actor 2", 0)}
-Roache's carvan was suppose to come in a few days ago. Third carvan to come go missing this month. If this keeps up, there will be bread riots.
*   [Blame the border patrols]
    Hmph. Most have been reassigned to the Northern border to fight the High Elves. How are they suppose to keep the roads with so few?
    **  [They are incompetent]
        Then why don't you join up and do something about it instead of -
    **  [Perhaps we should enlist and do something]
        It'll be dangerous... Bandits are a lot tougher than a bunch of street urchins. Maybe -
    **  [Drop the subject]

*   [Complain about the god King]
    Shhh. Not so loud. His absence only makes his priests more likely to strike out. Light above, they are already angry with how the war is going.
    **  [What if he does not return?]
        Then fewer and fewer will join the Order of Light and others will take its place. Theres no reason to - 
    **  [They are the reason for all these wars]
        He also unified the Five Kingdoms. Look, I'm - 
    **  [Drop the subject]
        Yeah, there are too many ears around. Let's go to - 

*   [Something must be done]
    Hah. Perhaps we should join up with a merchant. Probably pay well given the state of things.
//{place_Char("Actor 2", 0)}
-  A scuffle breaks out over a few drinks.
Looks like its time to move on. Best escalate it anymore, especially when { guard: we're|I'm } not on duty. Let's...
Shit. Look's like one of the gangs is making a play for power. Guard up!

//After the fight
Where's the damn guard? They should be here by now. 

Let's get out while we can.

->DONE

=== first_sandmite_encounter ===
VAR refugee_Food = 5
It's like something out of the old stories. Was that a sand mite?

Maybe. If that's true then there may be more. We must return to town and warn them.

What about our things back at camp? We might need them... but...

    *   [Let's go back. Let's try to to draw any more of them.]
        ~refugee_Food += 5
        You and Diana break camp as quietly as you can. Your heart pounds as you make your way out of the forest.
    
    *   [Let's go back, quickly!]
        ~refugee_Food += 2
        You reach camp safely with Diana and are relieved to see that it is still intact. You break camp and start to gather your things.
        
        You hear a screech in the distance. You gather the most important things and set off with Diana.
    
    *   [It's too dangerout. We don't know if theres more of them out there]
        You hurry out of the forest. Diana looks relieved to be out of there]

-   ->DONE

=== village ===
VAR warn_town = false
~temp improved_defense = false
~temp improved_supplies = false
//returns from a hunt having encountered a sand mite.
//meet woodelf
-{player}. You return quickly. Ill fortune or just getting old?

We saw a huge six legged beast! Five feet long, covered in bony plate. Is it a sand mite?

Yes, but this far North? How did they get here. Why did your kinsmen not bring warning? 
We must prepare. You must've seen one of their scouts. They will likely be upon us soon. A few hours at most.
Go warn your chieftan.

Already, she is beginning to fletch more arrows.
    *   (Defend)[I'll talk to him about defenses.]
    
    *   (Evacuate)[I'll get him to start evacuating the town.]
    
    *   (Advice)[Should we run or fight?]
        It is too late to run now. They are more activate at dusk. We may leave at dawn, earliest.

-   If they truely broke through Obrin, then we cannot hold here long. Diana, show me your arrows.

After sharing a quick glance, Diana starts to help Kerillian prepare arrows. You head to the great hall to warn the chieftan. It will be dusk in just a few hours. 
//move background
    You describe the beast you killed in the forest to the chieftan. All are shocked.How did they get past the Fort? 
    
    Gods be damned! What should we do?
*   {not Evacuate}[We must gather the townsmen and prepare defenses.]
    ~improved_defense = true
    Not many know how to fight. I'll gather those who can. We can put up some barricades as well. Gods let it be enough.

*   {not Defend} [We must gather our supplies while we can and leave.]
    ~improved_supplies = true
    I'll get everyone to start gathering their belongings.

*   {Advice} [They attack at dusk, but rest during the day. We can run then.]
    ~improved_defense = true
    So soon! I will gather the fighters. Gods send it is enough.

*   [We should send a message a ahead to warn the {city}.]
    ~warn_town = true
    Yes, yes. I'll start gathering those that can fight.
    
-   Already you can hear their shrieks drawing near. It will not be long now.

//Elf
She silently prays to her elven gods.

//Diana & main
Stay close to me.
Don't worry, I will.

//others?


-> DONE

=== village_aftermath ===
VAR leave_early = true
#The sandmites were defeated with relative ease.
That was not too bad. The stories made them seem tougher than they were. With a few walls we could easily hold them off.

#Kerillian
Those where the scouts. That was just the first flurries before the winter. Do not tell me you wish to stay.

#chief
They haven't been seen in hundreds of years. Who's to say its as bad? We can't just leave everything behind.

#Kerillian
{player}, are you seriously going to risk staying? 
*   [These are our homes. We can't just abandon them.]

*   {warn_town}[We sent a messenger to {city}. Perhaps they will send aid?]
    You will simply hope that help is coming?

*   [It is too dangerous to stay. I think we should leave.]
    Wise words. {chief} would do well to listen to them.
    #chief reponse
    You would really abandon everything we've built here?
    **  [No...I couldn't.]
        
    
    **  [We can rebuild.]
        You are free to leave, but you will be sorely missed.
        
        Not many others seem willing to join you. You consider your options.
        *** [Leave the town]
            ~   leave_early = true
        ->DONE
        
        *** [Stay]
            I'll stay another day to prepare.
            
*   [We can wait. A few days will allow us to pack for the journey.]
    A grave risk for a few trinkets.
    
-   I hope you know what you are doing. I must send word to my kin. Go prepare.
She leaves without waiting for a response.

There is no shortage of work to do. You consider your options, knowing that anything you decide will likely take the rest of the day.
*   (fortify)[Help fortify the walls.]

*   (clean)[Help clear out the rubble.]

*   (heal)[Tend to the wounded.]

*   (resupply)[Gather supplies.]

-   Night falls. You and a few other unwounded fighters gather to watch the forest.

    It is not long before you begin to see movement.

#fight

#chieftan
What a night. I thought that they would never stop coming.
#elf
This is just the beginning. Their scouts. It will get worse. We would be wise to leave.

But... No of course. I wish we had the strength to hold them back, like the heroes of legends. It is foolish to think so.
#elf
Legends must begin somehwere. But we alone cannot hold.
#cheiftan.
The {city} has walls and a garrison.
#elf 
Then let us move.
#chief
-We are still gathering supplies. We do not have enough to make it all the way to {city} on what we have now. Another day and we could get every cart in the city filled.

What do you think?

    *   [We need another day to get everyone ready to move]
        There is no guarentee that there will be people left tomorrow.
        
        We'll take that chance.
        
        Bah. You tempt fate.
    *   [We leave immediately]
        Then let us hope that we find kindness along the road.
-
->DONE