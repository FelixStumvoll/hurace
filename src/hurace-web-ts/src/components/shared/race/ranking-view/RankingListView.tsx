import React from 'react';
import { RankingListViewItem } from './RankingListViewItem';
import { HeaderCard } from '../../HeaderCard';
import { RaceRanking } from '../../../../models/RaceRanking';
import {
    NoListEntryText,
    ColumnFlex
} from '../../../../theme/CustomComponents';

export const RankingListView: React.FC<{ raceRanking: RaceRanking[] }> = ({
    raceRanking
}) => (
    <ColumnFlex>
        <HeaderCard
            headerText="Rangliste"
            contentStyles={
                raceRanking.length !== 0
                    ? { padding: 0, height: '100%', width: '100%' }
                    : {}
            }
        >
            {raceRanking.length !== 0 ? (
                <ColumnFlex>
                    {raceRanking.map(rr => (
                        <RankingListViewItem
                            key={rr.startNumber}
                            raceRanking={rr}
                        />
                    ))}
                </ColumnFlex>
            ) : (
                <NoListEntryText>Keine Ranglisteneinträge</NoListEntryText>
            )}
        </HeaderCard>
    </ColumnFlex>
);
