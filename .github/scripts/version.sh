#!/usr/bin/env bash
# One version for every package. Prints key=value lines, and appends them to $GITHUB_OUTPUT when set.

set -euo pipefail

FLOOR="${VERSION_FLOOR:-4.0.0}"
EVENT="${EVENT:-push}"
BRANCH="${BRANCH:-}"
PR_NUMBER="${PR_NUMBER:-}"
PUBLISH_INPUT="${PUBLISH_INPUT:-false}"
ACTOR="${ACTOR:-}"
RUN_NUMBER="${RUN_NUMBER:-0}"

bump_for() {
  local range="$1" messages sha
  messages=$(git log --format=%B "$range")
  if grep -qiE '\+semver:[[:space:]]*(breaking|major)' <<< "$messages"; then
    echo major
    return
  fi
  if grep -qiE '\+semver:[[:space:]]*(feature|minor)' <<< "$messages"; then
    echo minor
    return
  fi
  for sha in $(git rev-list "$range"); do
    if ! git log -1 --format=%B "$sha" | grep -qiE '\+semver:[[:space:]]*(none|skip)'; then
      echo patch
      return
    fi
  done
  echo none
}

next_version() {
  local major minor patch
  IFS=. read -r major minor patch <<< "$1"
  case "$2" in
    major) echo "$((major + 1)).0.0" ;;
    minor) echo "$major.$((minor + 1)).0" ;;
    *)     echo "$major.$minor.$((patch + 1))" ;;
  esac
}

at_least() {
  printf '%s\n%s\n' "$1" "$2" | sort -V | tail -n 1
}

if [ "$EVENT" = "pull_request" ]; then
  LABEL="pr${PR_NUMBER}"
else
  LABEL=$(printf '%s' "$BRANCH" | tr -c 'A-Za-z0-9' '-' | sed -E 's/-+/-/g; s/^-|-$//g')
fi

if [ "$ACTOR" = "dependabot[bot]" ] || [ "$EVENT" = "pull_request" ]; then
  KIND=build
elif [ "$BRANCH" = "main" ]; then
  KIND=release
elif [ "$BRANCH" = "develop" ] || [ "$PUBLISH_INPUT" = "true" ]; then
  KIND=prerelease
else
  KIND=build
fi

# Highest stable v tag by number, not by history, so develop also sees tags made on main's merge commits.
PREVIOUS=$(git tag --list 'v[0-9]*' | grep -E '^v[0-9]+\.[0-9]+\.[0-9]+$' | sort -V | tail -n 1 || true)

if [ -n "$PREVIOUS" ]; then
  RANGE="$PREVIOUS..HEAD"
  BUMP=$(bump_for "$RANGE")
  NEXT=$(at_least "$(next_version "${PREVIOUS#v}" "$BUMP")" "$FLOOR")
else
  BUMP=patch
  NEXT="$FLOOR"
fi

if [ "$KIND" = "release" ]; then
  if [ "$BUMP" = "none" ]; then
    KIND=build
    NOTE="Every commit since $PREVIOUS says +semver: none, so nothing is released."
    VERSION="${PREVIOUS#v}"
  else
    NOTE="Release, $BUMP bump from ${PREVIOUS:-nothing}."
    VERSION="$NEXT"
  fi
else
  VERSION="$NEXT-$LABEL.$RUN_NUMBER"
  if [ "$KIND" = "prerelease" ]; then
    NOTE="Prerelease towards $NEXT."
  else
    NOTE="Build only, nothing is published."
  fi
fi

RESULT=$(cat <<OUT
kind=$KIND
version=$VERSION
assembly=${VERSION%%-*}
previous=$PREVIOUS
note=$NOTE
OUT
)

echo "$RESULT"
if [ -n "${GITHUB_OUTPUT:-}" ]; then
  echo "$RESULT" >> "$GITHUB_OUTPUT"
fi
