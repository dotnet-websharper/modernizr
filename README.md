# WebSharper.Modernizr

This project provides WebSharper bindings to the Modernizr JavaScript library version 1.6.

Modernizr is a client-side JavaScript library that provides easy and portable checks
for emerging web technologies (CSS3, HTML 5). The full list of features that you can
check using this library is available in their [Website][modernizr].

## Getting started ##

Using Modernizr from WebSharper is very simple. Modernizr contains a single object with
static boolean properties for each feature. Checking the availability of the canvas feature
is as easy as writting:

    Modernizr.Canvas

[modernizr]: http://www.modernizr.com/
