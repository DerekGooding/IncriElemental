#!/bin/bash
# Script to run local Linux build test via Docker

IMAGE_NAME="incrielemental-linux-test"

echo "--- Building Docker Image: $IMAGE_NAME ---"
docker build -f Dockerfile.linux_test -t $IMAGE_NAME .

if [ $? -ne 0 ]; then
    echo "[FAIL] Docker build failed."
    exit 1
fi

echo "--- Running Linux Build Test ---"
docker run --rm $IMAGE_NAME

if [ $? -ne 0 ]; then
    echo "[FAIL] Linux build test inside Docker failed."
    exit 1
fi

echo "[SUCCESS] Linux build test passed!"
exit 0
