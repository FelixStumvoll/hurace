import React from 'react';
import { StartListViewItem } from './StartListViewItem';
import styled from 'styled-components';
import { HeaderCard } from '../../../shared/HeaderCard';
import { StartList } from '../../../../models/StartList';
import { NoListEntryText } from '../../../../theme/CustomComponents';

const StartListTable = styled.table`
    width: 100%;
    border-collapse: collapse;
`;

export const StartListView: React.FC<{ startList: StartList[] }> = ({
    startList
}) => {
    return (
        <HeaderCard headerText="Startliste:">
            {startList.length !== 0 ? (
                <StartListTable>
                    <thead>
                        <tr>
                            <th align="left">Startnr.</th>
                            <th align="left">Land</th>
                            <th align="left">Name</th>
                        </tr>
                    </thead>
                    <tbody>
                        {startList?.map(sl => (
                            <StartListViewItem
                                key={sl.startNumber}
                                startList={sl}
                            />
                        ))}
                    </tbody>
                </StartListTable>
            ) : (
                <NoListEntryText>Keine Startlisteneinträge</NoListEntryText>
            )}
        </HeaderCard>
    );
};
