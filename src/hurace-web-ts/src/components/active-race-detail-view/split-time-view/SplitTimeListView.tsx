import React from 'react';
import { TimeDifference } from '../../../models/TimeDifference';
import { HeaderCard } from '../../shared/HeaderCard';
import { NoListEntryText } from '../../../theme/CustomComponents';
import { StartList } from '../../../models/StartList';

export const SplitTimeListView: React.FC<{
    currentSkier: StartList | undefined;
    splitTimes: TimeDifference[];
}> = ({ currentSkier, splitTimes }) => {
    return (
        <HeaderCard
            headerText={
                currentSkier
                    ? `Zwischenzeiten - ${currentSkier.skier.firstName} ${currentSkier.skier.lastName}`
                    : 'Zwischenzeiten'
            }
        >
            {splitTimes.length !== 0 ? (
                <div></div>
            ) : (
                <NoListEntryText>Keine Zwischenzeiten</NoListEntryText>
            )}
        </HeaderCard>
    );
};
