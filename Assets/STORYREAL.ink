VAR speaker = ""
VAR portrait = ""
VAR background = ""

-> start

== start ==
~ speaker = "bob"
~ portrait = "happy"
~ background = "hospital"
Bob: Hey there... You made it. I wasn't sure you'd wake up. #glitch

+ What happened?
    ~ speaker = "bob"
    ~ portrait = "sad"
    Bob: You don't remember? That's normal. The others didn't either—at first. #whisper
    -> firstMission

+ Where am I?
    ~ speaker = "bob"
    ~ portrait = "happy"
    Bob: This used to be a hospital. Now it's... well, something else. #flicker
    -> firstMission

== firstMission ==
~ speaker = "luna"
~ portrait = "serious"
~ background = "hospital_hall"
Luna: Something's wrong in the east wing. Patients have been hearing growling from the walls. #whisper

+ We should check it out.
    -> acceptMission

+ That sounds like a bad idea...
    -> declineMission

== acceptMission ==
~ speaker = "luna"
~ portrait = "smile"
Luna: Good. Bring a flashlight. And try not to listen if they start whispering to you. #whisper
-> endMission

== declineMission ==
~ speaker = "luna"
~ portrait = "serious"
Luna: Then lock the door behind me. If I'm not back by dawn... don't open it. #glitch
-> endMission

== endMission ==
~ speaker = "bob"
~ portrait = "happy"
~ background = "hospital"
Bob: Either way... it always begins like this. You hear them too, right? #flicker
-> DONE
