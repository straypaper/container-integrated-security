#!/bin/bash

echo "Kerberos sidecar started at $(date)"

while true; do
    echo "Refreshing Kerberos ticket at $(date)..."
    KRB5_TRACE=$TRACE_OUT kinit "$PRINCIPAL" -k -t "$SECRETS/$KEYTAB"

    result=$?

    if [[ $result -eq 0 ]]; then
        echo "Success. Sleeping for $REKINIT_PERIOD seconds..."
    else
        echo "Failed to refresh Kerberos ticket. Result = $result"
        exit 1
    fi

    sleep "$REKINIT_PERIOD"
done
