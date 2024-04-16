$(document).ready(function () {
    var _carList = $('#carList');
    var _carApiUrl = 'https://localhost:5001/api/Listings';
    var _reviewList = $('#reviewList');
    var _reviewApiUrl = 'https://localhost:5001/api/Reviews';

    var loadReviewList = function () {
        const loadReviewsPromise = fetch(_reviewApiUrl, {
            mode: 'cors',
            headers: {
                'Accept': 'application/json'
            }
        });

        loadReviewsPromise.then((response) => {
            if (response.status === 200) {
                return response.json();
            } else {
                return Promise.reject(response);
            }
        })
            .then((reviews) => {
                _reviewList.empty();

                if (reviews.length === 0) {
                    _reviewList.append('<li>No reviews available.</li>');
                } else {
                    for (let i = 0; i < reviews.length; i++) {
                        _reviewList.append('<li>' + reviews[i].content + ' - Rating: ' +
                            reviews[i].rating + '</li>');
                    }
                }
            })
            .catch((response) => {
                console.log(`fetch review list; resp code: ${response.status}`);
            });
    };

    var loadCarList = function () {
        const loadCarsPromise = fetch(_carApiUrl, {
            mode: 'cors',
            headers: {
                'Accept': 'application/json'
            }
        });

        loadCarsPromise.then((response) => {
            if (response.status === 200) {
                return response.json();
            } else {
                return Promise.reject(response);
            }
        })
            .then((cars) => {
                _carList.empty();

                if (cars.length === 0) {
                    _carList.append('<li>No cars available.</li>');
                } else {
                    for (let i = 0; i < cars.length; i++) {
                        _carList.append('<li>' + cars[i].make + ' ' + cars[i].model + ' - Year: ' +
                            cars[i].year + ', Price: ' + cars[i].price + '</li>');
                    }
                }
            })
            .catch((response) => {
                console.log(`fetch car list; resp code: ${response.status}`);
            });
    };

    loadCarList();
    loadReviewList();

    $('#createListingBtn').click(function () {
        let newListing = {
            make: $('#make').val(),
            model: $('#model').val(),
            year: $('#year').val(),
            price: $('#price').val()
        };

        const createListingPromise = fetch(_carApiUrl, {
            method: 'POST',
            mode: 'cors',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(newListing)
        });

        createListingPromise.then((response) => {
            if (response.status === 201) {
                console.log('Listing created successfully.');
                loadCarList();
            } else {
                return Promise.reject(response);
            }
        })
            .catch((response) => {
                console.log(`create listing; resp code: ${response.status}`);
            });
    });

    $('#deleteListingBtn').click(function () {
        let listingId = $('#listingIdToDelete').val();

        const deleteListingPromise = fetch(`${_carApiUrl}/${listingId}`, {
            method: 'DELETE',
            mode: 'cors'
        });

        deleteListingPromise.then((response) => {
            if (response.status === 204) {
                console.log('Listing deleted successfully.');
                loadCarList();
            } else {
                return Promise.reject(response);
            }
        })
            .catch((response) => {
                console.log(`delete listing; resp code: ${response.status}`);
            });
    });

    $('#updateListingBtn').click(function () {
        let listingId = $('#listingIdToUpdate').val();
        let updatedListing = {
            make: $('#updatedMake').val(),
            model: $('#updatedModel').val(),
            year: $('#updatedYear').val(),
            price: $('#updatedPrice').val()
        };

        const updateListingPromise = fetch(`${_carApiUrl}/${listingId}`, {
            method: 'PUT',
            mode: 'cors',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(updatedListing)
        });

        updateListingPromise.then((response) => {
            if (response.status === 204) {
                console.log('Listing updated successfully.');
                loadCarList();
            } else {
                return Promise.reject(response);
            }
        })
            .catch((response) => {
                console.log(`update listing; resp code: ${response.status}`);
            });
    });

    $('#createReviewBtn').click(function () {
        let newReview = {
            content: $('#content').val(),
            rating: $('#rating').val()
        };

        const createReviewPromise = fetch(_reviewApiUrl, {
            method: 'POST',
            mode: 'cors',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(newReview)
        });

        createReviewPromise.then((response) => {
            if (response.status === 201) {
                console.log('Review created successfully.');
                loadReviewList();
            } else {
                return Promise.reject(response);
            }
        })
            .catch((response) => {
                console.log(`create review; resp code: ${response.status}`);
            });
    });

    $('#deleteReviewBtn').click(function () {
        let reviewId = $('#reviewIdToDelete').val();

        const deleteReviewPromise = fetch(`${_reviewApiUrl}/${reviewId}`, {
            method: 'DELETE',
            mode: 'cors'
        });

        deleteReviewPromise.then((response) => {
            if (response.status === 204) {
                console.log('Review deleted successfully.');
                loadReviewList();
            } else {
                return Promise.reject(response);
            }
        })
            .catch((response) => {
                console.log(`delete review; resp code: ${response.status}`);
            });
    });

    $('#updateReviewBtn').click(function () {
        let reviewId = $('#reviewIdToUpdate').val();
        let updatedReview = {
            content: $('#updatedContent').val(),
            rating: $('#updatedRating').val()
        };

        const updateReviewPromise = fetch(`${_reviewApiUrl}/${reviewId}`, {
            method: 'PUT',
            mode: 'cors',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(updatedReview)
        });

        updateReviewPromise.then((response) => {
            if (response.status === 204) {
                console.log('Review updated successfully.');
                loadReviewList();
            } else {
                return Promise.reject(response);
            }
        })
            .catch((response) => {
                console.log(`update review; resp code: ${response.status}`);
            });
    });
});
