# txt-files-binary-search-engin
find a word or set of words in txt file using AND OR NOT () operators with stop list (list of word the program ignores)

User guide  
  
Login:  
type "admin" as username and "1234" as password to login as admin. 
any other input logs the user in as a simple client.  
  
Search:  
the search supports only one level of brackets of type "()".  
the structure of the query should be "word OP word OP ..."  
available operators are AND, OR, NOT.  
there is no priority to any of the operators, only brackets do.  
there is also a built in stop list. this list contains words that should be 
ignored in the search and index process.  
to exit the search and go back to menu, enter "@".  
  
Document printing:  
a list of all available documents will be displayed. enter 
the number of the document you would like to see its content.  
  
Index printing:  
print the index table to see all the words scanned so far, 
including in what documents and what lines.  
  
Add document:  
(admin feature) add document to the system. the document will 
be backed up and the index table will be updated.  
notice that the word separation during the indexing process is by built in 
split characters. in case the new document contains additional characters 
they'll have to be added to the splitChars in the code  
  
Disable document:  
(admin feature) the document won't be displayed in the 
search results and its content won't be available to watch.  
  
Enable document:  
(admin feature) enable the document back.  
