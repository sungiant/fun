#!/bin/bash

set -x

rm -rf bin/
mkdir bin/

cp packages/NUnit.*/lib/nunit.framework.dll bin/

mcs \
-unsafe \
-debug \
-define:DEBUG \
-out:bin/fun.dll \
-target:library \
-recurse:source/fun/src/main/cs/*.cs \
-lib:bin/

mcs \
-unsafe \
-debug \
-define:DEBUG \
-out:bin/fun.test.dll \
-target:library \
-recurse:source/fun/src/test/cs/*.cs \
-lib:bin/ \
-reference:fun.dll \
-reference:nunit.framework.dll
