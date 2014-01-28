Feature: Publishing
	In order to publish new events
	As a publisher of content
	I want to tell a hub that I have events for a topic

Scenario: Publish new topic
	Given A hub is listening at "Http://localhost:4567"
	When I publish a new topic "Http://localhost:2113/UserEvents"
	Then the response status code should be: 204
	And the hub will contain my new topic "Http://localhost:2113/UserEvents"

Scenario: Publisher has updates
	Given A hub is listening at "Http://localhost:4567"
	And the hub contains the topic "Http://localhost:2113/UserEvents"
	Given a subscriber callback is configured to listen at "http://localhost:8080" with a prefix of "http://+:8080/"
	And the subscriber is listening for the topic "Http://localhost:2113/UserEvents"
	When the publisher notifies the hub of updates for topic "Http://localhost:2113/UserEvents"
	Then subscriber receives updates from the hub