import requests

class test_review_api:
    def __init__(self, base_url):
        self.base_url = base_url

    def get_all_reviews(self):
        response = requests.get(f"{self.base_url}/api/reviews")
        return response.json()

    def get_review(self, review_id):
        response = requests.get(f"{self.base_url}/api/reviews/{review_id}")
        if response.status_code == 404:
            return "Review not found"
        return response.json()

    def create_review(self, review_data):
        headers = {'Content-Type': 'application/json'}
        response = requests.post(f"{self.base_url}/api/reviews", json=review_data, headers=headers)
        if response.status_code == 201:
            return "Review created successfully", response.json()
        return "Error creating review", response.json()

    def update_review(self, review_id, review_data):
        headers = {'Content-Type': 'application/json'}
        response = requests.put(f"{self.base_url}/api/reviews/{review_id}", json=review_data, headers=headers)
        if response.status_code == 204:
            return "Review updated successfully"
        return "Error updating review", response.json()

    def delete_review(self, review_id):
        response = requests.delete(f"{self.base_url}/api/reviews/{review_id}")
        if response.status_code == 204:
            return "Review deleted successfully"
        return "Error deleting review", response.json()
        
base_url = 'http://localhost:5000/api' 
client = test_review_api(base_url)

# Test for get all reviews
print(client.get_all_reviews())

# Test for get a specific review
print(client.get_review(1))

# Test for post a review
review_data = {
    "UserId": 1,
    "ListingId": 1,
    "Content": "Great experience!",
    "Rating": 5,
    "ReviewTime": "2023-04-14T12:00:00Z"
}
print(client.create_review(review_data))

# Test for update a review
update_data = {
    "Id": 1,
    "UserId": 1,
    "ListingId": 1,
    "Content": "Updated review content.",
    "Rating": 4,
    "ReviewTime": "2023-04-15T12:00:00Z"
}
print(client.update_review(1, update_data))

# Test for deleting a review
print(client.delete_review(1))