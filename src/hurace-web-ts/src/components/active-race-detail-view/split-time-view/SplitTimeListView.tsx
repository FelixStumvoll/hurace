import React from 'react';
import { TimeDifference } from '../../../models/TimeDifference';
import { HeaderCard } from '../../shared/HeaderCard';
import { NoListEntryText, ColumnFlex } from '../../../theme/CustomComponents';
import { StartList } from '../../../models/StartList';
import { SplitTimeListItem } from './SplitTimeListItem';

export const SplitTimeListView: React.FC<{
    currentSkier: StartList | undefined;
    splitTimes: TimeDifference[];
}> = ({ currentSkier, splitTimes }) => {
    return (
        <ColumnFlex>
            <HeaderCard
                headerText={
                    currentSkier
                        ? `Zwischenzeiten - ${currentSkier.skier.firstName} ${currentSkier.skier.lastName}`
                        : 'Zwischenzeiten'
                }
                contentStyles={
                    splitTimes.length !== 0
                        ? { padding: 0, height: '100%', width: '100%' }
                        : {}
                }
            >
                {splitTimes.length !== 0 && currentSkier ? (
                    <ColumnFlex>
                        {splitTimes.map(sp => (
                            <SplitTimeListItem
                                timeDiff={sp}
                                key={sp.sensorNumber}
                            />
                        ))}
                    </ColumnFlex>
                ) : (
                    <NoListEntryText>
                        {currentSkier
                            ? 'Keine Zwischenzeiten'
                            : 'Kein Läufer auf der Strecke'}
                    </NoListEntryText>
                )}
            </HeaderCard>
        </ColumnFlex>
    );
};
