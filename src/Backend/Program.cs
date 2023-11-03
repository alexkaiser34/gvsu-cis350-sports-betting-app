using System;
using Backend.Services;

var app = new App();

await app.updateScores();
await app.updateOdds();
await app.updateWagers();



