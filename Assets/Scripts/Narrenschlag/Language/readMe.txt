-> Setup

1. Attach the LanguageManager Script to any GameObject

2. Create a Language Database via "Create>>Narrenschlag>>Language Database"

3. Plug the created Database into the LanguageManager Script on your GameObject

4. Demo: If you want to set the language update after a scene load, make sure to add the demo scene to the buildIndexMenu => 'File>>Build Settings...>>Add Open Scenes'

5. Done. Enjoy!



-> Usage

0. Before everything else you should create some words
> Open your used Database, open the list and increase the size
!> Then give any word an unique ID
> Last thing is to setup the words in the order of the Language enum.
        =>For example    order: english, german, spain

1. Get a reference via "narrenschlag.LanguageManager lang = narrenschlag.LanguageManager.singleton"

2. The language sets up autmaticly so just insert the id of the word you need.
        =>For example   Debug.Log(lang.GetWord(1)) => Debug.Log("Hello")



-> Modify

1. Languages
> The only thing u should modify is the language enum but keep in mind that the already setup words are in the order of the enum.
> To prevent this problem from happening just add a new language behind any other in the enum

-> Tips

1. Updating Language
> If you change the language you should always reload the scene. 
> You can try to modify the system so it updates everything without a scene reload but I don't suggest that
> You can set the system update the language AFTER loading a scene at >>> LanguageManager.cs/Line 40 <<< by just cutting it out :P