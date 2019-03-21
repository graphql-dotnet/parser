import { exec } from 'shelljs';
import Deferred from './Deferred';
import settings from './settings';

const branch = process.env.APPVEYOR_REPO_BRANCH || 'other';

function gitBranch() {
  const deferred = new Deferred();
  const git = `git rev-parse --abbrev-ref HEAD`;
  exec(git, (code, stdout, stderr)=> {
    if(code === 0) {
      deferred.resolve(stdout.trim());
    } else {
      deferred.reject(stderr);
    }
  });
  return deferred.promise;
}

export default function compile() {
  const deferred = new Deferred();

  console.log('Branch ' + branch);

  let versionSuffix = ''

  let versionSuffixSetting = settings.versionSuffix || ''

  if(branch !== 'master' && versionSuffixSetting.length == 0) {
    versionSuffixSetting = '-build'
  }

  if(versionSuffixSetting.length > 0) {
    versionSuffix = `-${versionSuffixSetting}-${settings.revision}`;
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

  return deferred.promise;
}
