#!/bin/bash

set -x

mono packages/NUnit.Runners.*/tools/nunit-console.exe bin/fun.test.dll
