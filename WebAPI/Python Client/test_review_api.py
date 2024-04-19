import requests
import json

BASE_URL = "http://localhost:5001/api/reviews"  

def test_create_review():
    url = BASE_URL
    headers = {'Content-Type': 'application/json'}
    data = {
        "content": "Great service!",
        "rating": 5,
        "username": "testuser"
    }
    response = requests.post(url, headers=headers, data=json.dumps(data))
    
    assert response.status_code == 201, f"Failed to create review: {response.text}"
    print("Review created successfully.")

def test_get_review():
    review_id = 1  # Example ID
    url = f"{BASE_URL}/{review_id}"
    response = requests.get(url)
    
    assert response.status_code == 200, f"Failed to fetch review: {response.text}"
    print("Review fetched successfully.")

def test_update_review():
    review_id = 1  
    url = f"{BASE_URL}/{review_id}"
    headers = {'Content-Type': 'application/json'}
    data = {
        "content": "Updated review content",
        "rating": 4,
        "username": "testuser"
    }
    response = requests.put(url, headers=headers, data=json.dumps(data))
    
    assert response.status_code == 204, f"Failed to update review: {response.text}"
    print("Review updated successfully.")

def test_delete_review():
    review_id = 1 
    url = f"{BASE_URL}/{review_id}"
    response = requests.delete(url)
    
    assert response.status_code == 204, "Failed to delete review."
    print("Review deleted successfully.")

if __name__ == "__main__":
    test_create_review()
    test_get_review()
    test_update_review()
    test_delete_review()
