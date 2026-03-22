// @ts-check
const eslint = require("@eslint/js");
const tseslint = require("typescript-eslint");
const angular = require("angular-eslint");
const importPlugin = require("eslint-plugin-import");
const prettierConfig = require("eslint-config-prettier");
const prettierPlugin = require("eslint-plugin-prettier");

module.exports = tseslint.config(
  {
    files: ["**/*.ts"],
    plugins: {
      "import": importPlugin,
      "prettier": prettierPlugin,
    },
    extends: [
      eslint.configs.recommended,
      ...tseslint.configs.recommended,
      ...tseslint.configs.stylistic,
      ...angular.configs.tsRecommended,
      prettierConfig,
    ],
    settings: {
      "import/resolver": {
        "typescript": true,
        "node": true
      }
    },
    processor: angular.processInlineTemplates,
    rules: {
      // === CRITICAL FOR LEARNING ===
      "@typescript-eslint/no-explicit-any": "error",
      "@typescript-eslint/explicit-function-return-type": "warn",
      "@typescript-eslint/no-unused-vars": "error",
      "@typescript-eslint/explicit-member-accessibility": ["error", {
        "accessibility": "explicit",
        "overrides": {
          "constructors": "no-public"
        }
      }],

      // Prevents common beginner mistakes
      "no-var": "error",
      "prefer-const": "error",
      "no-console": "warn",

      // === IMPORT RULES ===
      "import/order": ["warn", {
        "groups": ["builtin", "external", "internal", "parent", "sibling", "index"],
        "newlines-between": "always"
      }],
      "import/no-unresolved": "error",
      "import/no-duplicates": "warn",
      "import/first": "error",
      "import/newline-after-import": "warn",
      "import/no-named-as-default": "warn",

      // === PRETTIER ===
      "prettier/prettier": "error",

      // === ANGULAR RULES ===
      "@angular-eslint/directive-selector": [
        "error",
        {
          type: "attribute",
          prefix: "app",
          style: "camelCase",
        },
      ],
      "@angular-eslint/component-selector": [
        "error",
        {
          type: "element",
          prefix: "app",
          style: "kebab-case",
        },
      ],
      "@angular-eslint/use-lifecycle-interface": "error",
      "@angular-eslint/no-conflicting-lifecycle": "error",
      "@angular-eslint/no-output-native": "error",

      // === CODE QUALITY ===
      "max-len": ["warn", { "code": 100 }],
      "complexity": ["warn", 10],
      "max-lines-per-function": ["warn", 50],
    },
  },
  {
    files: ["**/*.html"],
    plugins: {
      "prettier": prettierPlugin,
    },
    extends: [
      ...angular.configs.templateRecommended,
      ...angular.configs.templateAccessibility,
      prettierConfig,
    ],
    rules: {
      "@angular-eslint/template/interactive-supports-focus": "error",
      "@angular-eslint/template/click-events-have-key-events": "error",
      "@angular-eslint/template/mouse-events-have-key-events": "error",
      "@angular-eslint/template/alt-text": "error",

      // === ANGULAR TEMPLATE BEST PRACTICES ===
      "@angular-eslint/template/no-negated-async": "error",
      "@angular-eslint/template/use-track-by-function": "warn",
      "@angular-eslint/template/no-call-expression": "warn",
      "@angular-eslint/template/cyclomatic-complexity": ["warn", { "maxComplexity": 5 }],

      "@angular-eslint/template/no-duplicate-attributes": "error",
      "@angular-eslint/template/conditional-complexity": ["warn", { "maxComplexity": 3 }],

      "prettier/prettier": "error",
    },
  }
);
