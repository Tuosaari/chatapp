Task 2 - IoC
===

Goal was to add inversion of control to system created in Task 1

## IoC Framework

Autofac was chosen as IoC container for the system. To be totally honest, this was done already early in Task 1. Having used and evaluated a couple IoC containers recently (mainly Ninject and Unity) Autofac seems to match my way of thinking and is thus an easy choice for me. In this simple system, probably any of the frameworks would have been enough, so going with Autofac was more due to familiarity. Additionaly Autofac has great ASP.NET Core support and documentation.

The latest version of Structure Map did seem interesting, but not as much as AutoFac. Might still need to give it a try at some point.

Using just ASP.NET Core depenceny injection would probably have been enough as well, but some helpful functionalities like named registeration helped a bit and simplified code (at the expense of configuration complexity).

