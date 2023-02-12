/** @type {import('ts-jest').JestConfigWithTsJest} */
module.exports = {
    preset: 'ts-jest',
    testEnvironment: 'node',
    reporters: ['default', ['github-actions', {silent: false}], 'summary']
};