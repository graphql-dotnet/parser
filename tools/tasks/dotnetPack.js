import { exec } from 'shelljs';
import Deferred from './Deferred';
import settings from './settings';

function gitBranch() {
  const deferred = new Deferred();
  const git = `git rev-parse --abbrev-ref HEAD`;
  exec(git, (code, stdout, stderr)=> {
    if(code === 0) {
      deferred.resolve(stdout);
    } else {
      deferred.reject(stderr);
    }
  });
  return deferred.promise;
}

export default function compile() {
  const deferred = new Deferred();

  gitBranch().then(branch => {

    let versionSuffix = ''

    let versionSuffixSetting = settings.versionSuffix || ''

    if(branch != 'master' && versionSuffixSetting.length == 0) {
      versionSuffixSetting = '-build'
    }

    if(versionSuffixSetting.length > 0) {
      versionSuffix = `${versionSuffixSetting}-${settings.revision}`;
    }

    const cmd = `dotnet pack src/GraphQLParser -o ${settings.artifacts} -c ${settings.target} /p:PackageVersion=${settings.version}${versionSuffix}`
    console.log(cmd);

    exec(cmd, (code, stdout, stderr)=> {
      if(code === 0) {
        deferred.resolve();
      } else {
        deferred.reject(stderr);
      }
    });
  });

  return deferred.promise;
}
