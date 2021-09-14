We all know about Hashing. They are like fingerprints. Usually they are expected to be as different as possible and so that's the way the hashing algorithms are created. In a 'Destructive' (:P) way. If a very small thing is different between two files or strings or anything like that, their hashes will be a lot different.

But there's another way of hashing. That also works as a fingerprint, but in a bit of a different mindset. If two entities are only a little different, their hashes will only be a little different. I've implemented one such Hash, called Perceptual Hashing in my code.

So this second way of hashing is an amazing way to compare things to find out how similar they are. First generate the hash (The Perceptual Hash) this time for 2 files, then find out the Hamming distance between the two hashes and VOILA! you now know how similar or dissimilar they are!

it is great for file comparison, it is great for searching images, may be you can use this to detect plagiarism, it's totally upto you :)
