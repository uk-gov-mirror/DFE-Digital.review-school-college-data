'use strict';

const merge = require('webpack-merge');
const baseConfig = require('./base.config.js');
const CleanObseleteChunks = require('webpack-clean-obsolete-chunks');

module.exports = merge(baseConfig, {
  devtool: 'source-map',
  mode: 'development',

  plugins: [
    // This cleans out outdated chunks between watch builds. The backend
    // expects to find only one version of each file in the `build` dir.
    new CleanObseleteChunks(),
  ],

});
