import React from 'react';
import { RankingViewItem } from './RankingViewItem';
import styled from 'styled-components';
import { HeaderCard } from '../../HeaderCard';
import { RaceRanking } from '../../../../models/RaceRanking';
import { NoListEntryText } from '../../../../theme/CustomComponents';

const RankingTable = styled.table`
    width: 100%;
    border-collapse: collapse;
`;

export const RankingView: React.FC<{ raceRanking: RaceRanking[] }> = ({
    raceRanking
}) => (
    <HeaderCard headerText="Rangliste">
        {raceRanking.length !== 0 ? (
            <RankingTable>
                <thead>
                    <tr>
                        <th align="left">Pos.</th>
                        <th align="left">Startnr.</th>
                        <th align="left">Land</th>
                        <th align="left">Name</th>
                        <th align="left">Zeit</th>
                        <th align="left">Rückstand</th>
                    </tr>
                </thead>
                <tbody>
                    {raceRanking?.map(rr => (
                        <RankingViewItem
                            key={rr.startList.startNumber}
                            raceRanking={rr}
                        />
                    ))}
                </tbody>
            </RankingTable>
        ) : (
            <NoListEntryText>Keine Ranglisteneinträge</NoListEntryText>
        )}
    </HeaderCard>
);
