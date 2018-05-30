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
				
				var count = console.log(result.rows[0].count);
				
				if(count > 0) {
					res.send(['Failure', 'This username or email already exist!']);
				}
				else {
					var pool2 = new pg.Pool(config);
					var authKey = (new tokenGenerator(tokenGenerator.BASE36)).generate();
					console.log(password);
					var insertQuery = {
						text: 'INSERT INTO "Users" ("Username", "Email", "Password", "AuthKey") VALUES ($1, $2, $3, $4)',
						values: [username, email, password, authKey],
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
						res.send(['Success', authKey]);
					});
				}
				done();
			});
	});
	pool.end();
});

app.post('/login', function(req, res) {
	console.log('loging user: ' + req.body);
});

app.param('authKey', function(req, res, next, authKey) {
    req.authKey = authKey;
    next();
});

app.get('/leaderboard/:authKey', function(req, res) {
	console.log('Level number: ' + req.authKey);
});

app.listen(port, function() {
	console.log(`Listening on port: ${port}`);
});
