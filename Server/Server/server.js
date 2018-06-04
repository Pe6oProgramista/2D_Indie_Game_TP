var express = require('express'),
	path = require('path'),
	bodyParser = require('body-parser'),
	tokenGenerator = require('uuid-token-generator'),
	pg = require('pg'),
	app = express();
var port = process.env.PORT || 5000;

const config = {
	// host: 'localhost',
	// port: 5000,
    // user: 'pepy',
    // database: 'InfinityCaveGame',
    // password: 'pepyypep',
	connectionString: process.env.DATABASE_URL, //"postgres://pepy:pepyypep@localhost:5000/InfinityCaveGame",//
};

app.use(bodyParser.json());
app.use(bodyParser.urlencoded({ extended: true }));

app.post('/register', function(req, res) {
	var data = JSON.parse(req.body['data']),
		username = data['username'],
		email = data['email'],
		password = data['password'];
	console.log('Registrating user: ', username);
	
	var query = {
		text: 'SELECT COUNT(1) FROM "Users" WHERE "Username" = $1 OR "Email" = $2',
		values: [username, email],
	}
	
	var pool = new pg.Pool(config);
	
	pool.connect(function(err, client, done) {
		if (err) {
			return console.error('error fetching client from pool', err);
		}
	    console.log('Connected to postgres! Getting schemas...');

		client
			.query(query, function(err, result) {
				if(err) {
					return console.error('error running query', err);
				}
				
				var count = result.rows[0].count;
				
				if(count > 0) {
					console.log('This username or email already exist!');
					res.send(['Failure', 'This username or email already exist!']);
				}
				else {
					var pool2 = new pg.Pool(config);
					var insertQuery = {
						text: 'INSERT INTO "Users" ("Username", "Email", "Password") VALUES ($1, $2, $3)',
						values: [username, email, password],
					}
					pool2.connect(function(err, client, done) {
						if (err) {
							return console.error('error fetching client from pool', err);
						}
						console.log('Connected to postgres! Getting schemas...');

						client
							.query(insertQuery, function(err, result) {
								if(err) {
									return console.error('error running query2', err);
								}
								console.log("Account added!");
								done();
							});
						res.send(['Success']);
					});
				}
				done();
			});
	});
	pool.end();
});

app.post('/login', function(req, res) {
	var data = JSON.parse(req.body['data']),
		username = data['username'],
		password = data['password'];
	console.log('Loging user: ' + username);
	
	var query = {
		text: 'SELECT "Password" FROM "Users" WHERE "Username" = $1',
		values: [username],
	}
	
	var pool = new pg.Pool(config);
	
	pool.connect(function(err, client, done) {
		if (err) {
			return console.error('error fetching client from pool', err);
		}
	    console.log('Connected to postgres! Getting schemas...');

		client
			.query(query, function(err, result) {
				if(err) {
					return console.error('error running query', err);
				}
				
				var count = result.rowCount;
				
				if(count == 0 || password != result.rows[0].Password) {
					res.send(['Failure', 'Invalid username or password!']);
				}
				else {
					var pool2 = new pg.Pool(config);
					var authKey = (new tokenGenerator(tokenGenerator.BASE36)).generate();
					console.log(authKey);
					var updateQuery = {
						text: 'UPDATE "Users" SET "AuthKey" = $1 WHERE "Username" = $2',
						values: [authKey, username],
					}
					pool2.connect(function(err, client, done) {
						if (err) {
							return console.error('error fetching client from pool', err);
						}
						console.log('Connected to postgres! Getting schemas...');

						client
							.query(updateQuery, function(err, result) {
								if(err) {
									return console.error('error running query2', err);
								}
								console.log("Loged in!");
								done();
							});
						res.send(['Success', authKey]);
					});
				}
				done();
			});
	});
	pool.end();
});

app.param('user', function(req, res, next, user) {
    req.user = user;
    next();
});

app.get('/levels/:user', function(req, res) {	
	console.log('Showing $1\'s levels...' + req.user);
	
	var query = {
		text: 'SELECT COUNT(1) FROM "Users" WHERE "AuthKey" = $1',
		values: [req.user],
	}
	
	var pool = new pg.Pool(config);
	
	pool.connect(function(err, client, done) {
		if (err) {
			return console.error('error fetching client from pool', err);
		}
	    console.log('Connected to postgres! Getting schemas...');

		client
			.query(query, function(err, result) {
				if(err) {
					return console.error('error running query', err);
				}
				
				var count = result.rowCount;
				if(count == 0) {
					res.send(['Failure', 'Connection error!']);
				}
				else {
					var pool2 = new pg.Pool(config);
					var getQuery = {
						text: 'SELECT "LastLevel" FROM "Users" WHERE "AuthKey" = $1',
						values: [req.user],
					}
					pool2.connect(function(err, client, done) {
						if (err) {
							return console.error('error fetching client from pool', err);
						}
						console.log('Connected to postgres! Getting schemas...');

						client
							.query(getQuery, function(err, result) {
								if(err) {
									return console.error('error running query2', err);
								}
								
								res.send(['Success', result.rows[0].LastLevel]);
								done();
							});
					});
				}
				
				done();
			});
	});
	pool.end();
});

app.param('levelNumber', function(req, res, next, levelNumber) {
    req.levelNumber = levelNumber;
    next();
});

app.get('/leaderboard/:levelNumber', function(req, res) {
	console.log('Level number: ' + req.levelNumber);
});

app.listen(port, function() {
	console.log(`Listening on port: ${port}`);
});
