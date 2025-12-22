// Migrated from .eslintrc.json for ESLint v9+
import angularEslintRecommended from '@angular-eslint/eslint-plugin';
import angularEslintTemplateRecommended from '@angular-eslint/eslint-plugin-template';
import prettierRecommended from 'eslint-plugin-prettier';
import typescriptEslintRecommended from '@typescript-eslint/eslint-plugin';
import tsParser from '@typescript-eslint/parser';

/** @type {import('eslint').Linter.FlatConfig[]} */
export default [
  {
    ignores: ['projects/**/*'],
  },
  {
    files: ['*.ts'],
    excludedFiles: ['**/*.spec.ts'],
    languageOptions: {
      parser: tsParser,
      parserOptions: {
        project: ['./tsconfig.json'],
        createDefaultProgram: true,
      },
    },
    plugins: {
      '@angular-eslint': angularEslintRecommended,
      '@typescript-eslint': typescriptEslintRecommended,
      'prettier': prettierRecommended,
    },
    rules: {
      curly: 'error',
      '@angular-eslint/directive-selector': [
        'error',
        { type: 'attribute', prefix: 'app', style: 'camelCase' },
      ],
      '@angular-eslint/component-selector': [
        'error',
        { type: 'element', prefix: 'app', style: 'kebab-case' },
      ],
      'no-eval': 'error',
      '@typescript-eslint/no-explicit-any': 'error',
      '@typescript-eslint/no-unused-vars': 'error',
      '@typescript-eslint/explicit-function-return-type': 'error',
      '@typescript-eslint/no-extraneous-class': 0,
      '@typescript-eslint/no-invalid-void-type': [
        'error',
        { allowInGenericTypeArguments: true },
      ],
      '@typescript-eslint/explicit-member-accessibility': [
        'error',
        {
          accessibility: 'explicit',
          overrides: {
            accessors: 'explicit',
            constructors: 'no-public',
            methods: 'explicit',
            properties: 'explicit',
            parameterProperties: 'explicit',
          },
        },
      ],
      '@angular-eslint/no-output-native': 'warn',
      '@angular-eslint/prefer-standalone': 'warn',
    },
  },
  {
    files: ['**/*.spec.ts'],
    languageOptions: {
      parser: tsParser,
      parserOptions: {
        project: ['./tsconfig.spec.json'],
        createDefaultProgram: true,
      },
    },
    plugins: {
      '@angular-eslint': angularEslintRecommended,
      '@typescript-eslint': typescriptEslintRecommended,
      'prettier': prettierRecommended,
    },
    rules: {
      '@angular-eslint/directive-selector': [
        'error',
        { type: 'attribute', prefix: 'app', style: 'camelCase' },
      ],
      '@angular-eslint/component-selector': [
        'error',
        { type: 'element', prefix: 'app', style: 'kebab-case' },
      ],
      'no-eval': 'error',
      '@typescript-eslint/no-explicit-any': 'off',
      '@typescript-eslint/no-unused-vars': 'error',
      '@typescript-eslint/explicit-function-return-type': 'error',
      '@typescript-eslint/no-extraneous-class': 0,
      '@typescript-eslint/no-invalid-void-type': [
        'error',
        { allowInGenericTypeArguments: true },
      ],
      '@typescript-eslint/explicit-member-accessibility': [
        'error',
        {
          accessibility: 'explicit',
          overrides: {
            accessors: 'explicit',
            constructors: 'no-public',
            methods: 'explicit',
            properties: 'explicit',
            parameterProperties: 'explicit',
          },
        },
      ],
      '@angular-eslint/no-output-native': 'warn',
      '@angular-eslint/prefer-standalone': 'warn',
    },
  },
  {
    files: ['*.html'],
    plugins: {
      '@angular-eslint/template': angularEslintTemplateRecommended,
    },
    rules: {
      "@angular-eslint/template/prefer-control-flow": "error"
    },
  },
];

