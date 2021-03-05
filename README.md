# MakeItEasy

Prototype of a library to make it easy to create
[systems under test](http://xunitpatterns.com/SUT.html) (SUTs) and to provide
access to the FakeItEasy Fakes used by the SUTs. Uses
[FakeItEasy](https://fakeiteasy.github.io/) to supply the SUT's collaborators.

## Simple Example

Suppose a system under test has a very long constructor:

```c#
public class VeryNeedySystem
{
    public VeryNeedySystem(
        DateTime time,
        String name,
        Priority priority,
        ICalendarService calendar,
        IEmailService email,
        ILogger logger,
        …)
    {
        …
    }
}
```

Some tests may not care about all those values. Instead of specifying all of them, just
send in the ones you care about:

```c#
using MakeItEasy;
…

public void TimeShouldBeRecent()
{
    var needy = Make.A<VeryNeedySystem>().From(new DateTime(2020, 12, 7));
    Assert.Throws(() => needy.ValidateState());
}
```

MakeItEasy will
1. find the public `VeryNeedySystem` constructor with all those arguments
2. match up the supplied date with the `time` parameter
3. make FakeItEasy
   [Dummies](https://fakeiteasy.readthedocs.io/en/stable/dummies/) for `name`,
   `calendar`, `email`, and
4. call the constructor, returning the result

It's possible to specify as few of the constructor arguments in the
`Make.A<…>.From(…)` as desired (even 0), or a great many. Of course, every
additional argument you supply means more work for you and less for MakeItEasy.


## Fancier Example, with Fake Creation

Sometimes you will care deeply about the arguments supplied to the SUT's
constructor, but you just don't want to create them yourself. Maybe you want the
SUT to use Fake services. In this case, MakeItEasy can make them for you, and
you can configure or interrogate them later:

```c#
public void ShouldSendEmail()
{
    var needy = Make.A<VeryNeedySystem>().From(DateTime.Now, out IEmailService fakeEmail);
    A.CallTo(() => fakeEmail.Send()).Returns(true);
    // use needy somehow
    A.CallTo(() => fakeEmail.Send()).MustHaveHappened();
}
```

As before, MakeItEasy locates an appropriate constructor, matches supplied
(non-`out`) arguments with constructor parameters, and provides Dummies for
unspecified parameters. But this time it makes a Fake for the `fakeEmail`
parameter and returns it for use in the test. The Fake can be configured and
interrogated as desired.

## Notes and Limitations

* The form of the API was suggested by @thomaslevesque, who has made other
  helpful comments during development of the prototype.
* There is no way to specify
[explicit creation options](https://fakeiteasy.readthedocs.io/en/stable/creating-fakes/#explicit-creation-options)
for the Fakes that are supplied to the system under test. If those are required,
you can create the Fake manually and pass it into the `From` method.
[Implicit Fake creation options](https://fakeiteasy.readthedocs.io/en/stable/implicit-creation-options/)
and
[custom Dummy creation rules](https://fakeiteasy.readthedocs.io/en/stable/custom-dummy-creation/)
can still be used.
* Parameters will be assigned type-compatible values in order (first parameter
  in the signature, second, etc.) as follows:
    * regular (non-`out`) arguments passed to the `From` method
    * Fakes created for the `From` method's `out`
    * Dummies

    This really only matters if there are repeated parameter types in the constructor's signature.
    
* The library is not yet well-protected against constructors that throw exceptions
