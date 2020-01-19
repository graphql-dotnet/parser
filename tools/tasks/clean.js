import { exec, rm } from 'shelljs';
import Deferred from './Deferred';

export default function clean() {
  const deferred = new Deferred();
  rm('-rf', `src/GraphQLParser/obj`);
  rm('-rf', `src/GraphQLParser/bin`);

  rm('-rf', `src/GraphQLParser.Tests/obj`);
  rm('-rf', `src/GraphQLParser.Tests/bin`);

  rm('-rf', `src/GraphQLParser.ApiTests/obj`);
  rm('-rf', `src/GraphQLParser.ApiTests/bin`);

  deferred.resolve();

  return deferred.promise;
}
