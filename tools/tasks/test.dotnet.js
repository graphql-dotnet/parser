import { exec, pushd, popd } from 'shelljs';
import Deferred from './Deferred';
import settings from './settings';

export default function testDotnet() {
  const deferred = new Deferred();

  const platform = process.platform === 'darwin'
    ? '-f netcoreapp2.2'
    : '';
  const test = `dotnet test ${platform} -c ${settings.target}`;

  pushd('src/GraphQLParser.Tests')
  console.log(test);

  exec(test, {async:true}, (code, stdout, stderr)=> {
    if(code === 0) {
      deferred.resolve();
    } else {
      deferred.reject(stderr);
    }
  });

  popd();

  const testApi = `dotnet test -f netcoreapp3.1 -c Debug`;

  pushd('src/GraphQLParser.ApiTests')
  console.log(testApi);

  exec(testApi, { async: true }, (code, stdout, stderr) => {
    if (code === 0) {
      deferred.resolve();
    } else {
      deferred.reject(stderr);
    }
  });

  popd();

  return deferred.promise;
}
