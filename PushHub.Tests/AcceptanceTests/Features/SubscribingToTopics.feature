Feature: Subscribing
	In order to receive notification of new events
	As a consumer of content
	I want to register for a topic with the hub

Scenario: Subscribing for a topic
	Given A hub is listening at "http://localhost:4567"
	And a publisher has registered a topic "http://localhost:2113/streams/UserEvents1"
	And a subscriber callback is configured to listen at "http://localhost:8080" with a prefix of "http://+:8080/"
	When I subscribe for the topic "http://localhost:2113/streams/UserEvents1"
	Then the subscription response status code should be: 204
	Then the hub calls me back to verify my subscription
	Then the hub contains a subscription with a callback url of "http://localhost:8080"
	#When I unsubscribe from the topic "http://localhost:2113/streams/UserEvents1"
	#Then the hub does not list the topic "http://localhost:2113/streams/UserEvents1"