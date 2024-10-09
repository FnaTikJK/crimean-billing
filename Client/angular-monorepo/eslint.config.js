const nx = require('@nx/eslint-plugin');
const { configs } = require('@eslint/js');

module.exports = [
  ...nx.configs['flat/base'],
  ...nx.configs['flat/typescript'],
  ...nx.configs['flat/javascript'],
  {
    ignores: ['**/dist'],
  },
  {
    files: ['**/*.ts', '**/*.tsx', '**/*.js', '**/*.jsx'],
    rules: {
      '@nx/enforce-module-boundaries': [
        'error',
        {
          enforceBuildableLibDependency: true,
          allow: ['^.*/eslint(\\.base)?\\.config\\.[cm]?js$'],
          depConstraints: [
            {
              sourceTag: '*',
              onlyDependOnLibsWithTags: ['*'],
            },
          ],
        },
      ],
    },
  },
  {
    files: ['**/*.ts', '**/*.tsx', '**/*.js', '**/*.jsx'],
    // Override or add rules here
    rules: {},
  },
  {
    files: ["*.component.html"],
    parser: "@angular-eslint/template-parser",
    parserOptions: {
      project: "./tsconfig.app.json",
      ecmaVersion: 2020,
      sourceType: "module",
    },
    plugins: ["@angular-eslint/template"]
  },
  {
    files: ["**/*.component.html"],
    ...configs.disableTypeChecked,
  }
];
